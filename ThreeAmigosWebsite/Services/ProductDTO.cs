using System;
using System.ComponentModel.DataAnnotations;

namespace ThreeAmigosWebsite.Services;

public class ProductDTO
{
    public int Id {get; set;}

    public string Name {get; set;} = string.Empty;
    
    public float Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public string BrandName { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public bool InStock { get; set; }

    public float PricePlus10Percent 
    {
        get { return Price * 1.1f; }
    }
}