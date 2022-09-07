using CsvHelper;
using examBaraaDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace examBaraaDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaraaController : ControllerBase
    {
        private readonly examsContext _db;
        public BaraaController(examsContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var data = _db.Categories.SelectMany(x => x.Subcategories/*.SelectMany(x => x.Items)*/).ToList();

            return Ok(data);
        }
        [HttpGet]
        public IActionResult CSV()
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"Items-{DateTime.Now.ToFileTime()}.csv");
            using (var streamWriter = new StreamWriter(path))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    var data = _db.Categories.SelectMany(x => x.Subcategories).SelectMany(x => x.Items).ToList();
                    csvWriter.WriteRecords(data);
                }
            }
            return Ok(path);
            //    var stream = File.OpenReadStream();
            //    using (var reader = new StreamReader(stream))
            //    using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            //    {

            //        var categoryRecords = new List<Category>();
            //        var subRecords = new List<Subcategory>();
            //        var ItemRecords = new List<Item>();
            //        var csv = csvReader.Read().ToString().Split(',');
            //        csvReader.ReadHeader();
            //        csvReader.Configuration.BadDataFound.Equals(true);

            //        while (csvReader.Read())
            //        {
            //            var catRecords = new Category
            //            {
            //                Name = csvReader.GetField("Categories")
            //            };
            //            categoryRecords.Add(catRecords);

            //            var fRecords = new Item
            //            {
            //                Name = csvReader.GetField("Items")
            //            };

            //            ItemRecords.Add(fRecords);
            //            ItemRecords.Select(x => x.Name).Distinct();

            //            var actionsRecords = new Subcategory
            //            {
            //                Name = csvReader.GetField("Subcategories")
            //            };
            //            subRecords.Add(actionsRecords);
            //            subRecords.Select(x => x.Name).Distinct();
            //        }
            //        categoryRecords.Select(x => x.Name).Distinct();
            //        var uniqueCategories = categoryRecords.GroupBy(p => p.Name)
            //                   .Select(grp => grp.First())
            //                   .ToArray();
            //        foreach (var category in uniqueCategories)
            //        {
            //            _db.Categories.Add(category);
            //            _db.SaveChanges();
            //        }
            //        foreach (var food in subRecords)
            //        {
            //            _db.Subcategories.Add(food);
            //            _db.SaveChanges();
            //        }

            //        foreach (var action in ItemRecords)
            //            _db.Items.Add(action);
            //        _db.SaveChanges();
            //    }
            //    return Ok(csvReader);
            //}

        //}
}
