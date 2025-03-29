using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Data;
using Game.UI;
using UnityEngine;

namespace Game.Entity
{
    public class TowerBox : MonoBehaviour
    {
        [SerializeField] private HPPanel hpView;
        [SerializeField] private SpriteRenderer model;
        [SerializeField] private SpriteRenderer blinkSpr;

        private int curIndex = -1;
        public int CurIndex => curIndex;
        private BoxData boxData = null;
        private int hp = 0;
        public int HP => hp;

        private Action<TowerBox> onBreakBox = null;

        private Color nonVisible = new Color(1f, 1f, 1f, 0f);
        private TweenerCore<Color, Color, ColorOptions> blinkTween = null;

        // 박스 데이터 초기화
        public void Init(BoxData data, int index, Action<TowerBox> onBreakBox = null)
        {
            this.boxData = data;
            model.sprite = boxData.GetModel();
            this.hp = boxData.GetHP();
            this.curIndex = index;

            if (onBreakBox != null)
            {
                this.onBreakBox += onBreakBox;
            }

            model.color = Color.white;
            hpView.UpdateView(hp, boxData.GetHP());
            ClearTween();
        }

        public void SetIndex(int index)
        {
            this.curIndex = index;
        }

        public void SubAction(Action<TowerBox> onBreakBox)
        {
            this.onBreakBox -= onBreakBox;
        }

        // 데미지 입었을 때 처리
        public void Damage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0;
                onBreakBox?.Invoke(this);
                gameObject.SetActive(false);
                StopAllCoroutines();
                return;
            }

            hpView.UpdateView(hp, boxData.GetHP());

            if (blinkTween != null)
            {
                return;
            }

            blinkTween = Blink(ClearTween);
        }

        // 데미지 입었을 때 반짝거리는 효과
        private TweenerCore<Color, Color, ColorOptions> Blink(Action onComplete = null)
        {
            blinkSpr.DOKill();
            Color color = Color.white;
            color.a = (150f / 255f);
            blinkSpr.color = color;
            return blinkSpr.DOColor(nonVisible, 0.2f).SetEase(Ease.Linear).OnComplete(() => onComplete?.Invoke());
        }

        public void Reset()
        {
            this.hp = boxData.GetHP();
            model.color = Color.white;
            ClearTween();
        }

        private void ClearTween()
        {
            blinkTween?.Kill();
            blinkTween = null;
            blinkSpr.color = nonVisible;
        }
    }
}
