using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrowMobile.Models;

public class ProductModel
{
    public int Id { get; set; }

    public string FarmerName { get; set; } = "";

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public int Qty { get; set; }

    public double Price { get; set; }

    public string Image { get; set; } = "";
}
