using SalesWebMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models.Services.Exceptions;

namespace SalesWebMVC.Models.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            //Acessa no banco a tabela de vendedores e converte em uma lista
            return _context.Seller.ToList();
        }

        public Seller FindById(int Id)
        {
            //eager loading
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == Id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            //Confere se não existe o Id do vendedor em questão
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            //Estamos fazendo algo importante aqui, transformando uma exceção de acesso à dados em uma excessão da camada de serviço, fazendo com que se mantenha e se respeite a ordem
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }


        }
    }
}
