using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class Equipment : UIWidget, IButton, ISelectable<int>
    {
        #region Enum&Class
        public enum SlotPosition
        {
            Center = 0,
            Top = 1,
            Bottom = 2
        }

        [System.Serializable]
        private class EquipmentCellData
        {
            public EquipmentCell cell;
            public SlotPosition position;
        }
        #endregion

        [Header("Cells")]
        [SerializeField] EquipmentCell[] cells;
        [SerializeField] SlotSprite slotSprite;

        EquipmentCellData[] cellData;
        Vector3[] slotPositions;

        public override bool Init()
        {
            if (base.Init() == false) return false;

            CacheSlotPositions();
            BindCellData();
            BindButtonEvents();
            return true;
        }

        #region Init Helpers
        void CacheSlotPositions()
        {
            slotPositions = new Vector3[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                slotPositions[i] = cells[i].transform.localPosition;
        }

        void BindCellData()
        {
            cellData = new EquipmentCellData[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                cellData[i] = new EquipmentCellData()
                {
                    cell = cells[i],
                    position = (SlotPosition)i
                };
                ApplyPosition(cellData[i]);
            }
        }

        public void BindButtonEvents()
        {
            for (int i = 0; i < cellData.Length; i++)
            {
                int index = i;
                Button button = cellData[i].cell.GetComponent<Button>();
                if (button == null) continue;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => Select(index));
            }
        }
        #endregion

        #region Selection Logic
        public void Select(int index)
        {
            UpdateSelectedCell(index);
            UpdateOtherCells(index);
            RefreshPositions();
        }

        void UpdateSelectedCell(int index)
        {
            cellData[index].position = SlotPosition.Center;
            cells[index].UpdateSlot(slotSprite.selectSprite,slotSprite.selectSize);
        }

        void UpdateOtherCells(int selectedIndex)
        {
            for (int i = 0; i < cellData.Length; i++)
            {
                if (i == selectedIndex) continue;

                if (cellData[(selectedIndex + 1) % cellData.Length] == cellData[i])
                    cellData[i].position = SlotPosition.Bottom;
                else
                    cellData[i].position = SlotPosition.Top;

                cells[i].UpdateSlot(slotSprite.unselectSprite,slotSprite.unselectSize);
            }
        }

        void RefreshPositions()
        {
            foreach (var data in cellData) ApplyPosition(data);
        }
        #endregion

        #region Position Helpers
        void ApplyPosition(EquipmentCellData data)
        {
            int posIndex = (int)data.position;
            if (posIndex < 0 || posIndex >= slotPositions.Length) return;

            data.cell.transform.localPosition = slotPositions[posIndex];
          
        }
        #endregion
    }
}
