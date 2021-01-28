using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using APCassandra.DTOs;
using Cassandra;
using Microsoft.AspNetCore.Mvc;

namespace APCassandra.Controllers
{
    public class SearchController : Controller
    {

        private readonly ISession _session;
        public SearchController(ISession session)
        {
            _session = session;
        }
        
        public IActionResult Index([FromQuery(Name = "Brand")] string brand, [FromQuery(Name = "model")] string model,
            [FromQuery(Name = "startYear")] string startYear, [FromQuery(Name = "endYear")] string endYear,
            [FromQuery(Name = "type")] string type, [FromQuery(Name = "fuel")] string fuel)
        {
            string query = "";
            //auto_by_brand
            if ((brand != null && brand != "") &&
                (model == null || model == "") && (type == null || type == "") && (fuel == null || fuel == ""))
            {
                query = $"SELECT * FROM auto_by_brand WHERE brand = '{Regex.Escape(brand)}' ";
            }
            //auto_by_brand_and_model
            else if ((brand != null && brand != "") && (model != null && model != "")
                && (type == null || type == "") && (fuel == null || fuel == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_model WHERE brand = '{Regex.Escape(brand)}' AND model = '{Regex.Escape(model)}' ";
            }
            //auto_by_brand_and_model_and_type
            else if ((brand != null && brand != "") && (model != null && model != "") && (type != null && type != "") &&
                (fuel == null || fuel == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_model_and_type WHERE brand = '{Regex.Escape(brand)}' AND model = '{Regex.Escape(model)}' ";
                query += $"AND type = '{Regex.Escape(type)}'";
            }
            //auto_by_brand_and_model_and_fuel_and_type
            else if ((brand != null && brand != "") && (model != null && model != "") && (type != null && type != "") && (fuel != null && fuel != ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_model_and_fuel_and_type WHERE brand = '{Regex.Escape(brand)}' AND model = '{Regex.Escape(model)}' ";
                query += $"AND type = '{Regex.Escape(type)}' AND fuel = '{Regex.Escape(fuel)}'";
            }
            //auto_by_brand_and_model_and_fuel
            else if ((brand != null && brand != "") && (model != null && model != "") && (fuel != null && fuel != "") &&
                (type == null || type == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_model_and_fuel WHERE brand = '{Regex.Escape(brand)}' AND model = '{Regex.Escape(model)}' ";
                query += $"AND fuel = '{Regex.Escape(fuel)}'";
            }
            //auto_by_fuel
            else if ((fuel != null && fuel != "") &&
                (model == null || model == "") && (type == null || type == "") && (brand == null || brand == ""))
            {
                query = $"SELECT * FROM auto_by_fuel WHERE fuel = '{Regex.Escape(fuel)}' ";
            }
            //auto_by_type
            else if ((type != null && type != "") &&
                (model == null || model == "") && (fuel == null || fuel == "") && (brand == null || brand == ""))
            {
                query = $"SELECT * FROM auto_by_type WHERE type = '{Regex.Escape(type)}' ";
            }
            //auto_by_fuel_and_type
            else if ((type != null && type != "") && (fuel != null && fuel != "")
                && (brand == null || brand == "") && (model == null || model == ""))
            {
                query = $"SELECT * FROM auto_by_fuel_and_type WHERE fuel = '{Regex.Escape(fuel)}' AND type = '{Regex.Escape(type)}' ";
            }
            // auto_by_brand_and_fuel
            else if ((brand != null && brand != "") && (fuel != null && fuel != "")
                && (type == null || type == "") && (model == null || model == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_fuel WHERE fuel = '{Regex.Escape(fuel)}' AND brand = '{Regex.Escape(brand)}' ";
            }
            //auto_by_brand_and_type
            else if ((brand != null && brand != "") && (type != null && type != "")
                && (fuel == null || fuel == "") && (model == null || model == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_type WHERE fuel = '{Regex.Escape(type)}' AND brand = '{Regex.Escape(brand)}' ";
            }
            //auto_by_brand_and_fuel_and_type
            else if ((brand != null && brand != "") && (fuel != null && fuel != "") && (type != null && type != "") &&
                (model == null || model == ""))
            {
                query = $"SELECT * FROM auto_by_brand_and_fuel_and_type WHERE brand = '{Regex.Escape(brand)}' AND fuel = '{Regex.Escape(fuel)}' ";
                query += $"AND type = '{Regex.Escape(type)}'";
            }
            //no query
            else
            {
                query = $"SELECT * FROM auto_by_id";
            }

            if (startYear != null && startYear != "" && int.TryParse(startYear, out _))
            {
                query += $" AND year > {startYear}";
            }
            if (endYear != null && endYear != "" && int.TryParse(endYear, out _))
            {
                query += $" AND year < {endYear}";
            }
            query += " ALLOW FILTERING;";
            var results = _session.Execute(query);
            var cars = new List<AutoDTO>();
            foreach (var car in results)
            {
                cars.Add(new AutoDTO()
                {
                    Brand = car.GetValue<string>("brand"),
                    Model = car.GetValue<string>("model"),
                    Id = car.GetValue<Guid>("id"),
                    Fuel = car.GetValue<string>("fuel"),
                    Type = car.GetValue<string>("type"),
                    Price = car.GetValue<int>("price"),
                    Year = car.GetValue<int>("year"),
                    Power = car.GetValue<int>("power"),
                    ShowImage = car.GetValue<string>("showimage")
                });
            }
            return View(cars);
        }
    }
}