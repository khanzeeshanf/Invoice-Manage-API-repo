using InvMang.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using InvMang.Service;

namespace InvMang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Invoice>> GetAllInvoices(int page = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var invoices = _invoiceService.GetAllInvoices(page, pageSize, fromDate, toDate);

            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public ActionResult<Invoice> GetInvoiceById(int id)
        {
            var invoice = _invoiceService.GetInvoiceById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpPost]
        public ActionResult<Invoice> AddInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                return BadRequest("Invoice cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _invoiceService.AddInvoice(invoice);

            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest("Id and Invoice.Id do not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingInvoice = _invoiceService.GetInvoiceById(id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            _invoiceService.UpdateInvoice(invoice);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteInvoice(int id)
        {
            var existingInvoice = _invoiceService.GetInvoiceById(id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            _invoiceService.DeleteInvoice(id);

            return NoContent();
        }
    }
}
