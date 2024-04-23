using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanellController : MonoBehaviour
{
    public Text _PriceText;
    public Text _PriceTextDisabled;
    public Text _DiscountText;
    public Text _DiscountAmountText;

    public void _UpdatePannel(Database.PackageInfo iPackageInfo)
    {
        _PriceTextDisabled.text= _PriceText.text = string.Format("{0} {1}", iPackageInfo.package_price, ab.TomanText);
        if (iPackageInfo.package_discount==0)
        {
            _DiscountText.gameObject.SetActive(false);
            _DiscountAmountText.gameObject.SetActive(false);
            _PriceTextDisabled.gameObject.SetActive(false);
            _PriceText.gameObject.SetActive(true);


        }
        else
        {
            _PriceText.gameObject.SetActive(false);
            _DiscountText.gameObject.SetActive(true);
            _DiscountAmountText.gameObject.SetActive(true);
            
            _PriceTextDisabled.gameObject.SetActive(true);
            _DiscountText.text = string.Format("{2} {0} {1}", Mathf.RoundToInt((iPackageInfo.package_price*(100-iPackageInfo.package_discount))/100), ab.TomanText,ab.AfterDiscountText);
            _DiscountAmountText.text = string.Format("{0}% \n {1}",iPackageInfo.package_discount,ab.DiscountText);
        }
    }
}
