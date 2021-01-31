using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using APCassandra.Models;
using Cassandra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        private readonly Cassandra.ISession _session;

        public AutoController(Cassandra.ISession session)
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
        public IActionResult Edit([FromQuery(Name = "id")] string id)
        {
            GetUserAutoList();
            Auto = UserAutoList.Where(x => x.Id.ToString() == id).FirstOrDefault();
            if (Auto == null)
                return NotFound("Car do not exist");
            return View(this);
        }

        public IActionResult Detail([FromQuery(Name = "id")] string id)
        {
            this.Auto = new AutoModel();
            string query = $"SELECT * FROM auto_by_id WHERE id={Guid.Parse(id)};";
            var rs = _session.Execute(query);
            var temp = rs.First();
            Auto.Brand = temp.GetValue<string>("brand");
            Auto.Model = temp.GetValue<string>("model");
            Auto.Year = temp.GetValue<int>("year");
            Auto.Price = temp.GetValue<int>("price");
            Auto.Color = temp.GetValue<string>("color");
            Auto.Contact = temp.GetValue<string>("contact");
            Auto.Power = temp.GetValue<int>("power");
            Auto.Type = temp.GetValue<string>("type");
            Auto.Volume = temp.GetValue<int>("volume");
            Auto.ShowImage = temp.GetValue<string>("showimage");
            Auto.ImagesList = temp.GetValue<List<string>>("imageslist");
            return View(this);
        }

        [Authorize]
        public IActionResult MyCars()
        {
            GetUserAutoList();
            return View(UserAutoList);
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
                temp.ShowImage = row.GetValue<string>("showimage"); ;
                temp.Type = row.GetValue<string>("type");
                temp.Year = row.GetValue<int>("year");
                temp.UserId = userID;
                UserAutoList.Add(temp);
            }
        }


        [BindProperty]
        public List<IFormFile> FormFiles { get; set; }

        public async Task<IActionResult> OnCreateAdAsync()
        {
            Auto.Id = Guid.NewGuid();
            Auto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Auto.EquipmentList != null && Auto.EquipmentList.Length > 0)
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


            // image upload
            List<string> imageNames = new List<string>();
            int imageCount = 0;
            foreach (var formFile in FormFiles)
            {
                if (formFile.Length > 0)
                {
                    string imageName = Auto.Id.ToString() + imageCount + DateTime.Now.Ticks + ".jpg";
                    var filePath = Path.Combine("wwwroot\\images",
                        imageName);
                    imageCount++;
                    imageNames.Add(imageName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            string imageNamesString = "";
            if (imageNames.Count > 0)
            {

                for (int i = 0; i < imageNames.Count; i++)
                {
                    imageNames[i] = "'" + imageNames[i] + "'";
                }

                imageNamesString = string.Join(',', imageNames);
            }

            _session.Execute("INSERT INTO auto_by_id (id, brand, color, contact, equipmentlist, fuel, imagesList, model, power, price, showimage, type, userid, volume, year, description)" +
                $" VALUES (" +
                $"{Auto.Id}, " +
                $"'{Auto.Brand}', " +
                $"'{Auto.Color}', " +
                $"'{Auto.Contact}', " +
                $"[{Auto.EquipmentList}], " +
                $"'{Auto.Fuel}', " +
                $"[{imageNamesString}], " +
                $"'{Auto.Model}', " +
                $"{Auto.Power}, " +
                $"{Auto.Price}," +
                $" {imageNames[0]}, " +
                $"'{Auto.Type}', " +
                $"'{Auto.UserId}', " +
                $"{Auto.Volume}, " +
                $"{Auto.Year}, " +
                $"'{Auto.Description}'); ");

            _session.Execute("INSERT INTO auto_by_brand (brand, year, id, fuel, model, price, showimage, type)" +
                $" VALUES (" +
                $"'{Auto.Brand}', " +
                $"{Auto.Year}, "+
                $"{Auto.Id}, " +
                $"'{Auto.Fuel}', " +
                $"'{Auto.Model}', " +
                $"{Auto.Price}," +
                $" {imageNames[0]}, " +
                $"'{Auto.Type}'); "
                );

            _session.Execute("INSERT INTO auto_by_brand_and_model (brand, year, id, model, fuel, showimage, price, type)" +
                $" VALUES (" +
                $"'{Auto.Brand}', " +
                $"{Auto.Year}, " +
                $"{Auto.Id}, " +
                $"'{Auto.Model}', " +
                $"'{Auto.Fuel}', " +
                $" {imageNames[0]}, " +
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
                $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
               $" {imageNames[0]}, " +
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
            Auto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Auto.EquipmentList != null && Auto.EquipmentList.Length > 0)
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
                $"volume={Auto.Volume}, " +
                $"description='{Auto.Description}' " +
                $"WHERE id={Auto.Id};");

            _session.Execute("UPDATE auto_by_brand" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $"WHERE id={Auto.Id} " +
                $"AND brand ='{Auto.Brand}' " +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_brand_and_model" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand ='{Auto.Brand}' " +
                $"AND model='{Auto.Model}' " +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_brand_and_model_and_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand ='{Auto.Brand}' " +
                $"AND model='{Auto.Model}' " +
                $"AND year={Auto.Year} " +
                $"AND fuel='{Auto.Fuel}';");

            _session.Execute("UPDATE auto_by_brand_and_model_and_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand ='{Auto.Brand}' " +
                $"AND model='{Auto.Model}' " +
                $"AND type='{Auto.Type}' " +
                $"AND year={Auto.Year} " +
                $"AND fuel='{Auto.Fuel}';");

            _session.Execute("UPDATE auto_by_brand_and_model_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand ='{Auto.Brand}' " +
                $"AND model='{Auto.Model}' " +
                $"AND type='{Auto.Type}' " +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND fuel='{Auto.Fuel}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND fuel='{Auto.Fuel}'" +
                $"AND type='{Auto.Type}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND type='{Auto.Type}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_brand_and_fuel" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand='{Auto.Brand}'" +
                $"AND fuel='{Auto.Fuel}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_brand_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand='{Auto.Brand}'" +
                $"AND type='{Auto.Type}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_brand_and_fuel_and_type" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND brand='{Auto.Brand}'" +
                $"AND type='{Auto.Type}'" +
                $"AND fuel='{Auto.Fuel}'" +
                $"AND year={Auto.Year};");

            _session.Execute("UPDATE auto_by_user" +
                $" SET " +
                $"price={Auto.Price}," +
                $" showimage='' " +
                $" WHERE id={Auto.Id} " +
                $"AND user='{Auto.UserId}';");

            return Redirect("/");
        }
    }
}