using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin.App_Code.DAL
{
    public class DataRespository
    {
        private SpainBS_CampusEntities context = new SpainBS_CampusEntities();


        //public List<int> GetAniosFacturas()
        //{
        //    try
        //    {
        //        var anios = context.iNF
        //            .Select(f => f.fecha_emision.Year)
        //            .Distinct()
        //            .ToList();
        //        var maxAnio = anios.Max();
        //        anios.Add(maxAnio + 1);
        //        return anios.OrderByDescending(a => a).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}

    }
}