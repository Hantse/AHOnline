using FrontendApp.Models;

namespace FrontendApp.Datas
{
    public class AuctionHouseData
    {
        public static List<CategoryModel> Categories
        {
            get
            {
                return new List<CategoryModel>()
                {
                    new CategoryModel()
                    {
                        Key = "Quiver"
                    },
                    new CategoryModel()
                    {
                        Key = "Quest"
                    },
                    new CategoryModel()
                    {
                        Key = "Weapon",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "OneHandAxe"
                            },
                            new CategoryModel()
                            {
                                Key = "TwoHandAxe"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Armor",
                        SubCategories = new List<CategoryModel>()
                        {

                            new CategoryModel()
                            {
                                Key = "Miscellaneous"
                            },
                            new CategoryModel()
                            {
                                Key = "Cloth"
                            },
                            new CategoryModel()
                            {
                                Key = "Leather"
                            },
                            new CategoryModel()
                            {
                                Key = "Mail"
                            },
                            new CategoryModel()
                            {
                                Key = "Plate"
                            },
                            new CategoryModel()
                            {
                                Key = "Shield"
                            },
                            new CategoryModel()
                            {
                                Key = "Libram"
                            },
                            new CategoryModel()
                            {
                                Key = "Idol"
                            },
                            new CategoryModel()
                            {
                                Key = "Totem"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Container",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Container"
                            },
                            new CategoryModel()
                            {
                                Key = "Soul Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Herb Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Enchanting Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Engineering Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Gem Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Minning Bag"
                            },
                            new CategoryModel()
                            {
                                Key = "Leatherworking Bag"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Consumable",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Food & Drink"
                            },
                            new CategoryModel()
                            {
                                Key = "Potion"
                            },
                            new CategoryModel()
                            {
                                Key = "Elixir"
                            },
                            new CategoryModel()
                            {
                                Key = "Flask"
                            },
                            new CategoryModel()
                            {
                                Key = "Bandage"
                            },
                            new CategoryModel()
                            {
                                Key = "Item Enhancement"
                            },
                            new CategoryModel()
                            {
                                Key = "Scroll"
                            },
                            new CategoryModel()
                            {
                                Key = "Other"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Trade Goods",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Elemental"
                            },
                            new CategoryModel()
                            {
                                Key = "Cloth"
                            },new CategoryModel()
                            {
                                Key = "Leather"
                            },
                            new CategoryModel()
                            {
                                Key = "Metal & Stone"
                            },
                            new CategoryModel()
                            {
                                Key = "Meat"
                            },
                            new CategoryModel()
                            {
                                Key = "Herb"
                            },
                            new CategoryModel()
                            {
                                Key = "Enchanting"
                            },
                            new CategoryModel()
                            {
                                Key = "Jewelcrafting"
                            },
                            new CategoryModel()
                            {
                                Key = "Elemental"
                            },
                            new CategoryModel()
                            {
                                Key = "Devices"
                            },
                            new CategoryModel()
                            {
                                Key = "Explosives"
                            },
                            new CategoryModel()
                            {
                                Key = "Materials"
                            },
                            new CategoryModel()
                            {
                                Key = "Other"
                            },
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Projectile",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Arrow"
                            },
                            new CategoryModel()
                            {
                                Key = "Bullet"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Recipe",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Book"
                            },
                            new CategoryModel()
                            {
                                Key = "Leatherworking"
                            },
                            new CategoryModel()
                            {
                                Key = "Tailoring"
                            },
                            new CategoryModel()
                            {
                                Key = "Engineering"
                            },
                            new CategoryModel()
                            {
                                Key = "Blacksmithing"
                            },
                            new CategoryModel()
                            {
                                Key = "Cooking"
                            },
                            new CategoryModel()
                            {
                                Key = "Alchemy"
                            },
                            new CategoryModel()
                            {
                                Key = "First Aid"
                            },
                            new CategoryModel()
                            {
                                Key = "Enchanting"
                            },
                            new CategoryModel()
                            {
                                Key = "Fishing"
                            },
                            new CategoryModel()
                            {
                                Key = "Jewelcrafting"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Gem",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Red"
                            },
                            new CategoryModel()
                            {
                                Key = "Blue"
                            },
                            new CategoryModel()
                            {
                                Key = "Yellow"
                            },
                            new CategoryModel()
                            {
                                Key = "Purple"
                            },
                            new CategoryModel()
                            {
                                Key = "Green"
                            },
                            new CategoryModel()
                            {
                                Key = "Orange"
                            },
                            new CategoryModel()
                            {
                                Key = "Meta"
                            },
                            new CategoryModel()
                            {
                                Key = "Simple"
                            },
                            new CategoryModel()
                            {
                                Key = "Prismatic"
                            }
                        }
                    },
                    new CategoryModel()
                    {
                        Key = "Miscellaneous",
                        SubCategories = new List<CategoryModel>()
                        {
                            new CategoryModel()
                            {
                                Key = "Junk"
                            },
                            new CategoryModel()
                            {
                                Key = "Reagent"
                            },
                            new CategoryModel()
                            {
                                Key = "Pet"
                            },
                            new CategoryModel()
                            {
                                Key = "Holiday"
                            },
                            new CategoryModel()
                            {
                                Key = "Other"
                            },
                            new CategoryModel()
                            {
                                Key = "Mount"
                            }
                        }
                    }
                };
            }
        }
    }
}
