using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text itemLevel;
        [SerializeField]
        private TMP_Text itemLevelUse;
        [SerializeField]
        private TMP_Text itemType;
        [SerializeField]
        private TMP_Text itemDescription;

        private void Awake()
        {
            ResetDesription();
            gameObject.SetActive(false);
        }
        public void ResetDesription()
        {
            itemImage.gameObject.SetActive(false);
            itemName.text = "";
            itemLevel.text = "";
            itemLevelUse.text = "";
            itemType.text = "";
            itemDescription.text = "";

        }
        public void SetDescription(string _itemName, Sprite sprite, string _itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemName.text = _itemName;
            itemImage.sprite = sprite;
            //itemLevel.text = _itemStats[ItemStat.Level].ToString();
            //itemLevelUse.text = _itemStats[ItemStat.LevelRequire].ToString();
            //itemType.text = _itemType.ToString();
            //_itemDescription += "\n";
            //foreach (var item in _itemStats)
            //{
            //    _itemDescription += $"{item.Key} : {item.Value} \n";
            //}
            itemDescription.text = _itemDescription;
        }

    }
}