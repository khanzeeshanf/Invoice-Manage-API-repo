using InvMang.Model;
using System.Collections.Generic;
using System;

namespace InvMang.Service
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetAllInvoices(int page, int pageSize, DateTime? fromDate, DateTime? toDate);
        void AddInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        Invoice GetInvoiceById(int id);
        void DeleteInvoice(int id);
    }
}
