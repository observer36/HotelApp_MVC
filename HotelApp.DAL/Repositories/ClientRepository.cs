﻿using HotelApp.DAL.Interfaces;
using HotelApp.DAL.Entities;
using HotelApp.DAL.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.DAL.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(HotelDbContext context) : base(context)
        {
        }
        public void LoadActiveOrders(Client client, PaymentStateEnum paymentState = default)
        {
            IQueryable<ActiveOrder> orders = context.Entry(client).Collection(p => p.ActiveOrders).Query()
                .Include(p => p.HotelRoom).ThenInclude(p => p.TypeComfort)
                .Include(p => p.HotelRoom).ThenInclude(p => p.TypeSize);
            if (paymentState != 0)
                orders = orders.Where(p => p.PaymentState == paymentState);
            orders.Load();
            //context.Entry(client).Collection(p => p.ActiveOrders).Query().Where(p => p.PaymentState == paymentState).Load();
        }
        public void LoadHotelRooms(Client client)
        {
            context.Entry(client).Collection(p => p.HotelRooms).Load();
        }
        public Client FindByPhoneNumber(string number)
        {
            return context.Clients.Where(p => p.PhoneNumber == number).SingleOrDefault();
        }
    }
}
