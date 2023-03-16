using System.ComponentModel.DataAnnotations;

namespace MyNetwork.ViewModels
{
    public enum Months
    {
        [Display(Name = "Январь")]
        Jan = 1,
        //код здесь
        [Display(Name = "Февраль")]
        Feb,
        [Display(Name = "Март")]
        Mar,
        [Display(Name = "Апрель")]
        Apr,
        [Display(Name = "Май")]
        May,
        [Display(Name = "Июнь")]
        Jun,
        [Display(Name = "Июль")]
        Jul,
        [Display(Name = "Август")]
        Aug,
        [Display(Name = "Сентябрь")]
        Sep,
        [Display(Name = "Октябрь")]
        Oct,
        [Display(Name = "Ноябрь")]
        Nov,
        [Display(Name = "Декабрь")]
        Dec
    }
}
