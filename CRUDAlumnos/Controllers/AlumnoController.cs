using CRUDAlumnos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAlumnos.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            try
            {
                int edad = 18;
                string sql = @"select a.Id, a.Nombres, a.Apellido, a.Edad, a.Sexo, a.FechaRegistro, c.Nombre as NombreCiudad
                                from Alumno a
                                join Ciudad c on a.CodCiudad = c.Id";
                                //where a.Edad > @edad";


                using (var db = new AlumnosContext())
                    {
                    //    var data = from a in db.Alumno
                    //               join c in db.Ciudad on a.CodCiudad equals c.Id
                    //               select new AlumnoCE()
                    //               {
                    //                   Id = a.Id,
                    //                   Nombres = a.Nombres,
                    //                   Apellido = a.Apellido,
                    //                   Edad = a.Edad,
                    //                   Sexo = a.Sexo,
                    //                   NombreCiudad = c.Nombre,
                    //                   FechaRegistro = a.FechaRegistro

                    //               };

                    //    return View(data.ToList());
                    //}
                    return View(db.Database.SqlQuery<AlumnoCE>(sql, new SqlParameter("@edad",edad)).ToList());
             }
             }
            catch (Exception)
            {

                throw;
            }

        }
        
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno a)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new AlumnosContext())
                {
                    a.FechaRegistro = DateTime.Now;
                    db.Alumno.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("","Error al agregar al alumno"+ ex.Message);
                    return View();
            }
            
        }
        public ActionResult Agregar2()
        {
            return View();
        }

        public ActionResult ListaCiudades()
        {
            using(var db = new AlumnosContext())
            {
                return PartialView(db.Ciudad.ToList());
            }
        }

        public ActionResult Editar(int id)
        {
            try
            {
                using(var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno alum = db.Alumno.Find(id);
                    return View(alum);
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Alumno a)
        {
            try
            {
                using (var db = new AlumnosContext())
                {
                    Alumno alum = db.Alumno.Find(a.Id);
                    alum.Nombres = a.Nombres;
                    alum.Apellido = a.Apellido;
                    alum.Edad = a.Edad;
                    alum.Sexo = a.Sexo;
                    alum.CodCiudad = a.CodCiudad;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al agregar al alumno" + ex.Message);
                return View();
            }
        }
        public ActionResult Detalles(int id)
        {
            using (var db = new AlumnosContext())
            {                
                Alumno alum = db.Alumno.Find(id);
                return View(alum);
            }
        }

        public ActionResult EliminarAlumno(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno alum = db.Alumno.Find(id);
                db.Alumno.Remove(alum);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public static string NombreCiudad(int CodCiudad)
        {
            using(var db = new AlumnosContext())
            {
                return db.Ciudad.Find(CodCiudad).Nombre;
            }
        }

    }
}