using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game.Entity
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private List<TowerBox> towerBoxList;
        [SerializeField] private Hero hero;
        [SerializeField] private Transform truck;
        private List<TowerBox> useBoxList = null;

        private void Awake()
        {
            for (int i = 0; i < towerBoxList.Count; i++)
            {
                towerBoxList[i].Init(new Data.BoxData(1), i, OnBoxBreak);
            }

            useBoxList = towerBoxList.ToList();
        }

        private void OnBoxBreak(TowerBox box)
        {
            for (int i = box.CurIndex; i < useBoxList.Count; i++)
            {
                if (i - 1 < 0 || useBoxList[i].CurIndex < i)
                {
                    continue;
                }

                useBoxList[i].SetIndex(i - 1);
                useBoxList[i].DOKill();
                int index = i;
                if (i == useBoxList.Count - 1)
                {
                    int lastIndex = i;
                    TowerBox target = useBoxList[lastIndex];
                    useBoxList[i].transform.DOMove(useBoxList[index - 1].transform.position, 0.6f).SetEase(Ease.OutBack).OnUpdate(() =>
                    {
                        hero.MoveToLastBox(target.transform.position);
                    });
                }
                else
                {
                    useBoxList[i].transform.DOMove(useBoxList[index - 1].transform.position, 0.6f).SetEase(Ease.OutBack);
                }
            }

            useBoxList.Remove(box);
            if (useBoxList.Count == 0)
            {
                hero.MoveToLastBox(truck.position);
            }
        }

        public TowerBox GetClosetBox(Vector3 position, float arrange)
        {
            TowerBox targetBox = null;
            float minDistance = 100f;

            foreach (var box in useBoxList)
            {
                float distance = Vector2.SqrMagnitude((Vector2)box.transform.position - (Vector2)position);

                if (targetBox == null)
                {
                    targetBox = box;
                    minDistance = distance;
                    continue;
                }

                if (minDistance > distance)
                {
                    targetBox = box;
                    minDistance = distance;
                }
            }

            return targetBox;
        }

        public Hero TryGetHero()
        {
            if (useBoxList.Count > 0)
            {
                return null;
            }

            return hero;
        }
    }
}