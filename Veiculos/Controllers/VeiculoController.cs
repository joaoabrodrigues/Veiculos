﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Veiculos.Models;

namespace Veiculos.Controllers
{
    /*
    A classe WebApiConfig pode requerer alterações adicionais para adicionar uma rota para esse controlador. Misture essas declarações no método Register da classe WebApiConfig conforme aplicável. Note que URLs OData diferenciam maiúsculas e minúsculas.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Veiculos.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<VeiculoModel>("Veiculo");
    builder.EntitySet<PessoaModel>("Pessoa"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class VeiculoController : ODataController
    {
        private ContextoDb db = new ContextoDb();

        // GET: odata/Veiculo
        [EnableQuery]
        public IQueryable<VeiculoModel> GetVeiculo()
        {
            return db.Veiculo;
        }

        // GET: odata/Veiculo(5)
        [EnableQuery]
        public SingleResult<VeiculoModel> GetVeiculoModel([FromODataUri] int key)
        {
            return SingleResult.Create(db.Veiculo.Where(veiculoModel => veiculoModel.id == key));
        }

        // PUT: odata/Veiculo(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<VeiculoModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VeiculoModel veiculoModel = db.Veiculo.Find(key);
            if (veiculoModel == null)
            {
                return NotFound();
            }

            patch.Put(veiculoModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(veiculoModel);
        }

        // POST: odata/Veiculo
        public IHttpActionResult Post(VeiculoModel veiculoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Veiculo.Add(veiculoModel);
            db.SaveChanges();

            return Created(veiculoModel);
        }

        // PATCH: odata/Veiculo(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<VeiculoModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VeiculoModel veiculoModel = db.Veiculo.Find(key);
            if (veiculoModel == null)
            {
                return NotFound();
            }

            patch.Patch(veiculoModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(veiculoModel);
        }

        // DELETE: odata/Veiculo(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            VeiculoModel veiculoModel = db.Veiculo.Find(key);
            if (veiculoModel == null)
            {
                return NotFound();
            }

            db.Veiculo.Remove(veiculoModel);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Veiculo(5)/Proprietario
        [EnableQuery]
        public SingleResult<PessoaModel> GetProprietario([FromODataUri] int key)
        {
            return SingleResult.Create(db.Veiculo.Where(m => m.id == key).Select(m => m.Proprietario));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VeiculoModelExists(int key)
        {
            return db.Veiculo.Count(e => e.id == key) > 0;
        }
    }
}
