using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using APCassandra.Models;
using Cassandra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APCassandra.Controllers
{
    public class AutoController : Controller
    {
        [BindProperty]
        public AutoModel Auto { get; set; }
        public List<AutoModel> UserAutoList { get; set; }
        [BindProperty]
        public int SelectedIndex { get; set; }

        private readonly ISession _session;

        public AutoController(ISession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        public IActionResult Delete()
        {
            GetUserAutoList();
            return View(this);
        }

        [Authorize]
        public IActionResult Edit([FromQuery(Name="id")] string id)
        {
            GetUserAutoList();
            Auto = UserAutoList.Where(x => x.Id.ToString() == id).First();
            return View(this);
        }

        private void GetUserAutoList()
        {
            UserAutoList = new List<AutoModel>();
            string userID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rs = _session.Execute($"SELECT * FROM auto_by_user WHERE user='{userID}'");

            foreach (var row in rs)
            {
                AutoModel temp = new AutoModel();
                temp.Id = row.GetValue<Guid>("id");
                temp.Brand = row.GetValue<string>("brand");
                temp.Fuel = row.GetValue<string>("fuel");
                temp.Model = row.GetValue<string>("model");
                temp.Price = row.GetValue<int>("price");
                temp.ShowImage = "";
                temp.Type = row.GetValue<string>("type");
                temp.Year = row.GetValue<int>("year");
                temp.UserId = userID;
                UserAutoList.Add(temp);
            }
        }

        public IActionResult OnCreateAd()
        {
            Auto.Id = Guid.NewGuid();
            Auto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Auto.EquipmentList.Length > 0)
            {
                Auto.EquipmentList = Auto.EquipmentList.Replace(" ", "");
                string[] str = Auto.EquipmentList.Split(',');

                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = "'" + str[i] + "'";
                }

                Auto.EquipmentList = string.Join(',', str);
            }
            else
            {
                Auto.EquipmentList = "";
            }
            _session.Execute("INSERT INTO auto_by_id (id, brand, color, contact, equipmentlist, fuel, imagesList, model, power, price, showimage, type, userid, volume, year)" +
                $" VALUES (" +
                $"{Auto.Id}, " +
                $"'{Auto.Brand}', " +
                $"'{Auto.Color}', " +
                $"'{Auto.Contact}', " +
                $"[{Auto.EquipmentList}], " +
                $"'{Auto.Fuel}', " +
                $"[''], " +
                $"'{Auto.Model}', " +
                $"{Auto.Power}, " +
                $"{Auto.Price}," +
                $" '', " +
                $"'{Auto.Type}', " +
                $"'{Auto.UserId}', " +
                $"{Auto.Volume}, " +
                $"{Auto.Year}); ");

            _session.Execute("INSERT INTO auto_by_brand (brand, year, id, fuel, model, price, showimage, type)" +
                $" VALUES (" +
                $"'{Auto.Brand}', " +
                $"{Auto.Year}, "+
                $"{Auto.Id}, " +
                $"'{Auto.Fuel}', " +
                $"'{Auto.Model}', " +
                $"{Auto.Price}," +
                $" '', " +
                $"'{Auto.Type}'); "
                );

            _session.Execute("INSERT INTO auto_by_brand_and_model (brand, year, id, model, fuel, showimage, price, type)" +
                $" VALUES (" +
                $"'{Auto.Brand}', " +
                $"{Auto.Year}, " +
                $"{Auto.Id}, " +
                $"'{Auto.Model}', " +
                $"'{Auto.Fuel}', " +
                $" '', " +
                $"{Auto.Price}," +
                $"'{Auto.Type}'); "
                );

            _session.Execute("INSERT INTO auto_by_brand_and_model_and_fuel (brand, year, id, model, fuel, showimage, price, type)" +
                $" VALUES (" +
                $"'{Auto.Brand}', " +
                $"{Auto.Year}, " +
                $"{Auto.Id}, " +
                $"'{Auto.Model}', " +
                $"'{Auto.Fuel}', " +
                $" '', " +
                $"{Auto.Price}," +
                $"'{Auto.Type}'); "
                );

            _session.Execute("INSERT INTO auto_by_brand_and_model_and_fuel_and_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_brand_and_model_and_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_fuel (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_fuel_and_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_brand_and_fuel (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_brand_and_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_brand_and_fuel_and_type (brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            _session.Execute("INSERT INTO auto_by_user (user, brand, year, id, model, fuel, showimage, price, type)" +
               $" VALUES (" +
               $"'{Auto.UserId}', " +
               $"'{Auto.Brand}', " +
               $"{Auto.Year}, " +
               $"{Auto.Id}, " +
               $"'{Auto.Model}', " +
               $"'{Auto.Fuel}', " +
               $" '', " +
               $"{Auto.Price}," +
               $"'{Auto.Type}'); "
               );

            return Redirect("/");
        }

        public IActionResult OnDeleteAd()
        {
            GetUserAutoList();
            Guid toDelete = this.UserAutoList[SelectedIndex].Id;

            _session.Execute($"DELETE FROM auto_by_id" +
                $" WHERE id={toDelete};");

            _session.Execute($"DELETE FROM auto_by_brand" +
                $" WHERE id={toDelete} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_brand_and_model" +
                $" WHERE id={toDelete} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_brand_and_model_and_fuel" +
                $" WHERE id={toDelete} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND year={UserAutoList[SelectedIndex].Year} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}';");

            _session.Execute($"DELETE FROM auto_by_brand_and_model_and_fuel_and_type" +
                $" WHERE id={toDelete} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND type='{UserAutoList[SelectedIndex].Type}' " +
                $"AND year={UserAutoList[SelectedIndex].Year} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}';");

            _session.Execute($"DELETE FROM auto_by_brand_and_model_and_type" +
                $" WHERE id={toDelete} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND type='{UserAutoList[SelectedIndex].Type}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_fuel" +
                $" WHERE id={toDelete} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_fuel_and_type" +
                $" WHERE id={toDelete} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_type" +
                $" WHERE id={toDelete} " +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_brand_and_fuel" +
                $" WHERE id={toDelete} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_brand_and_type" +
                $" WHERE id={toDelete} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_brand_and_fuel_and_type" +
                $" WHERE id={toDelete} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute($"DELETE FROM auto_by_user" +
                $" WHERE id={toDelete} " +
                $"AND user='{UserAutoList[SelectedIndex].UserId}';");

            return Redirect("/");
        }

        public IActionResult OnEditAd()
        {
            GetUserAutoList();
            Auto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Auto.Id = UserAutoList[SelectedIndex].Id;

            if (Auto.EquipmentList.Length > 0)
            {
                Auto.EquipmentList = Auto.EquipmentList.Replace(" ", "");
                string[] str = Auto.EquipmentList.Split(',');

                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = "'" + str[i] + "'";
                }

                Auto.EquipmentList = string.Join(',', str);
            }
            else
            {
                Auto.EquipmentList = "";
            }

            _session.Execute("UPDATE auto_by_id" +
                $" SET " +
                $"color='{Auto.Color}', " +
                $"contact='{Auto.Contact}', " +
                $"equipmentlist=[{Auto.EquipmentList}], " +
                $"imagesList=[''], " +
                $"power={Auto.Power}, " +
                $"price={Auto.Price}," +
                $" showimage='', " +
                $"volume={Auto.Volume} " +
                $"WHERE id={UserAutoList[SelectedIndex].Id};");

            _session.Execute("UPDATE auto_by_brand" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $"WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_brand_and_model" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_brand_and_model_and_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND year={UserAutoList[SelectedIndex].Year} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}';");

            _session.Execute("UPDATE auto_by_brand_and_model_and_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND type='{UserAutoList[SelectedIndex].Type}' " +
                $"AND year={UserAutoList[SelectedIndex].Year} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}';");

            _session.Execute("UPDATE auto_by_brand_and_model_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand ='{UserAutoList[SelectedIndex].Brand}' " +
                $"AND model='{UserAutoList[SelectedIndex].Model}' " +
                $"AND type='{UserAutoList[SelectedIndex].Type}' " +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_brand_and_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_brand_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_brand_and_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND brand='{UserAutoList[SelectedIndex].Brand}'" +
                $"AND type='{UserAutoList[SelectedIndex].Type}'" +
                $"AND fuel='{UserAutoList[SelectedIndex].Fuel}'" +
                $"AND year={UserAutoList[SelectedIndex].Year};");

            _session.Execute("UPDATE auto_by_user" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={UserAutoList[SelectedIndex].Id} " +
                $"AND user='{UserAutoList[SelectedIndex].UserId}';");

            return Redirect("/");
        }
    }
}