using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnciclopediaDarwin.Data.Entidades;

namespace EnciclopediaDarwin.Logica
{
    public interface IEspecieServicio
    {
        List<Especie> ObtenerTodos();
        void Insertar(Especie especie);

        List<TipoEspecie> ObtenerTipoEspecies();
        Especie ObtenerPorId(int id);
        void Eliminar(Especie especie);
        void Actualizar(Especie especie);
    }
    public class EspecieServicio : IEspecieServicio
    {
        private _20222CEnciclopediaDarwinContext _context;
        public EspecieServicio(_20222CEnciclopediaDarwinContext context)
        {
            _context = context;
        }

        public void Actualizar(Especie especie)
        {
            var especieEnDb = _context.Especies.Find(especie.IdEspecie);
            if (especieEnDb == null) return;
            
            especieEnDb.Nombre = especie.Nombre;
            especieEnDb.PesoPromedioKg = especie.PesoPromedioKg;
            especieEnDb.EdadPromedioAños = especie.EdadPromedioAños;
            especieEnDb.IdTipoEspecie = especie.IdTipoEspecie;

            _context.SaveChanges();
        }

        public void Eliminar(Especie especie)
        {
            _context.Especies.Remove(especie);
            _context.SaveChanges();
        }

        public void Insertar(Especie especie)
        {
            _context.Especies.Add(especie);
            _context.SaveChanges();
        }

        public Especie ObtenerPorId(int id)
        {
            return _context.Especies.Find(id);
        }

        public List<TipoEspecie> ObtenerTipoEspecies()
        {
            return _context.TipoEspecies.OrderBy(te => te.Nombre).ToList();
        }

        public List<Especie> ObtenerTodos()
        {
            return _context.Especies.ToList();
        }

        public void VerTipo()
        {
            var tipoEspecie = _context.TipoEspecies
                                    .Where(t => t.Nombre.Contains(""))
                                    .Select(t1 => t1).FirstOrDefault();

            List<Especie> especies = tipoEspecie.Especies.ToList();

            foreach (Especie es in especies)
            {
                Console.Write(es.Nombre);
            }
        }

        public List<Especie> ObtenerPorBusqueda(string filtro)
        {
            // 1) Origen de datos
            // _context con 3 especies

            // 2) Creación de consulta - Sintaxis de consulta
            // IQueryable<Especie>
            var especiesQuery = (from e in _context.Especies
                           join t in _context.TipoEspecies
                           on e.IdTipoEspecie equals t.IdTipoEspecie
                           where t.Nombre.Contains(filtro)
                           orderby e.Nombre
                           select e);

            // 2) Creación de consulta - Sintaxis de metodo
            // IQueryable<Especie>
            var especiesMethod = _context.Especies
                           .Where(e => e.Nombre.Contains(filtro))
                           .Select(e1 => e1)
                           .OrderBy(e2 => e2.Nombre);

            // 2 y 3) Creación de consulta y ejecución en una sola acción
            // List<Especie>
            // var especiesMethod = _context.Especies
            //                .Where(e => e.Nombre.Contains(filtro))
            //                .Select(e1 => e1)
            //                .OrderBy(e2 => e2.Nombre).List();

            List<Especie> especies = especiesQuery.ToList();

            // Agregado de 4ta Especie
            Especie newEsp = new Especie();
            _context.Especies.Add(newEsp);
            _context.SaveChanges();

            // 3) Ejecución de consulta
            foreach (Especie es in especiesMethod)
            {
                Console.Write(es.IdTipoEspecieNavigation.Nombre);
            }

            // 3) Ejecución de consulta
            //return especiesMethod.ToList();

            return especies;
        }

    }
}
