using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.ViewModels.FoodModel
{
    public class FoodViewModel
    {
        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Cate { get; set; }

        public string? Image { get; set; }
    }
}
