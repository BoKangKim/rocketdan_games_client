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
                useBoxList[i].transform.DOMove(useBoxList[index - 1].transform.position, 0.6f).SetEase(Ease.OutBack);
            }

            useBoxList.Remove(box);
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
    }
}