using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geotest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly GeoContext ctx;
        public ValuesController(GeoContext ctx)
        {
            this.ctx = ctx;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<Local> Get()
        {
            var ls = ctx.Locais.FromSql("Select Id, Nome, Ponto.ToString() as Ponto From Locais where ponto is not null");
            return ls;
        }

        // GET api/values/5
        [HttpGet("proximos")]
        public IEnumerable<Local> GetProximo([FromQuery] double lng, [FromQuery] double lat)
        {
            var ls = ctx.Locais
                       .FromSql(@"
                                DECLARE @g geography = 'POINT ('+@lng+' '+@lat+')';
                                Select Id, Nome, Ponto.ToString() as Ponto 
                                From Locais 
                                where ponto is not null
                                Order by Ponto.STDistance(@g)",
                                new SqlParameter("lng", lng.ToString()),
                                new SqlParameter("lat", lat.ToString()));
            return ls;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]LocalViewModel value)
        {
            var local = new Local
            {
                Nome = value.Nome
            };
            ctx.Add(local);
            ctx.SaveChanges();
            var update = $"Update Locais set Ponto = geography::STGeomFromText('POINT ('+@lng+' '+@lat+')', 4326) WHERE id = @id";
            ctx.Database.ExecuteSqlCommand(update,
                    new SqlParameter("id", local.Id),
                    new SqlParameter("lng", value.Location.Lng.ToString()),
                    new SqlParameter("lat", value.Location.Lat.ToString())
                    );
        }
    }

    public class LocalViewModel
    {
        public string Nome { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
