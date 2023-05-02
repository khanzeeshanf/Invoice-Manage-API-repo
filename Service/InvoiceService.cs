using InvMang.Model;
using InvMang.Repository;
using System.Collections.Generic;
using System;
using System.Linq;

namespace InvMang.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Invoice> GetAllInvoices(int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            var query = _unitOfWork.InvoiceRepository.GetAll();

            if (fromDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate <= toDate.Value);
            }

            var invoices = query.OrderBy(i => i.InvoiceDate)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            return invoices;
        }

        public void AddInvoice(Invoice invoice)
        {
            invoice.SubTotal = invoice.InvoiceDetails.Sum(d => d.Total);
            invoice.Tax = invoice.InvoiceDetails.Sum(d => d.Tax * d.Total / 100);
            invoice.GrandTotal = invoice.SubTotal + invoice.Tax;

            _unitOfWork.InvoiceRepository.Add(invoice);
            _unitOfWork.Save();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            var existingInvoice = _unitOfWork.InvoiceRepository.GetAll().FirstOrDefault(i => i.Id == invoice.Id);

            if (existingInvoice != null)
            {
                existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
                existingInvoice.InvoiceDate = invoice.InvoiceDate;
                existingInvoice.DueDate = invoice.DueDate;
                existingInvoice.Status = invoice.Status;
                existingInvoice.BillTo = invoice.BillTo;
                existingInvoice.InvoiceNote = invoice.InvoiceNote;

                existingInvoice.InvoiceDetails.Clear();
                foreach (var detail in invoice.InvoiceDetails)
                {
                    existingInvoice.InvoiceDetails.Add(detail);
                }

                existingInvoice.SubTotal = invoice.InvoiceDetails.Sum(d => d.Total);
                existingInvoice.Tax = invoice.InvoiceDetails.Sum(d => d.Tax * d.Total / 100);
                existingInvoice.GrandTotal = existingInvoice.SubTotal + existingInvoice.Tax;

                _unitOfWork.InvoiceRepository.Update(existingInvoice);
                _unitOfWork.Save();
            }
        }

        public Invoice GetInvoiceById(int id)
        {
            return _unitOfWork.InvoiceRepository.GetAll().FirstOrDefault(i => i.Id == id);
        }

        public void DeleteInvoice(int id)
        {
            var existingInvoice = _unitOfWork.InvoiceRepository.GetAll().FirstOrDefault(i => i.Id == id);

            if (existingInvoice != null)
            {
                _unitOfWork.InvoiceRepository.Delete(existingInvoice);
                _unitOfWork.Save();
            }
        }
    }
}
