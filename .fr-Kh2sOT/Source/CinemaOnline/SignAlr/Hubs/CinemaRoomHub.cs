using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BELibrary.Entity;
using BELibrary.Extendsions;
using BELibrary.Models.View;
using Microsoft.AspNet.SignalR;

namespace CinemaOnline.signalr.hubs
{
    public class CinemaRoomHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ReserveTicketDto> reserveTickets = new ConcurrentDictionary<string, ReserveTicketDto>();
        public void Send(ReserveTicketDto reserveTicket)
        {
            var reserveTicketId = reserveTicket.GetId();
            var seatIds = reserveTickets.Where(rt => !rt.Value.GetId().Equals(reserveTicket.GetId())).SelectMany(rt => rt.Value.SeatIds).Distinct().ToList();
            Clients.Client(reserveTicket.HubId).addMessage(new ReserveTicketDto { SeatIds = seatIds });
            if (reserveTickets.Count == 0)
            {
                reserveTicket.StartBooking = DateTime.Now.AddMinutes(2).ToString();
                reserveTickets.TryAdd(reserveTicket.HubId, reserveTicket);
                Clients.Client(reserveTicket.HubId).addMessage(reserveTicket);
                foreach (var rt in reserveTickets)
                {
                    if (rt.Value.HubId.Equals(reserveTicket.HubId))
                    {
                        continue;
                    }
                    if (rt.Value.GetId().Equals(reserveTicketId))
                    {
                        Clients.Client(rt.Key).addMessage(reserveTicket);
                        continue;
                    }
                    Clients.Client(rt.Key).addMessage(reserveTicket);
                }
            }
            else
            {
                var reserveExisted = reserveTickets.FirstOrDefault(rt => rt.Value.HubId.Equals(reserveTicket.HubId));
                if (reserveExisted.Value == null)
                {
                    reserveTicket.StartBooking = DateTime.Now.AddMinutes(2).ToString();
                    reserveTickets.TryAdd(reserveTicket.HubId, reserveTicket);
                    Clients.Client(reserveTicket.HubId).addMessage(reserveTicket);
                }
                else
                {
                    reserveTickets.TryUpdate(reserveTicket.HubId, reserveTicket, reserveExisted.Value);
                }
                foreach (var rt in reserveTickets)
                {
                    if (rt.Value.HubId.Equals(reserveTicket.HubId))
                    {
                        continue;
                    }
                    if (rt.Value.GetId().Equals(reserveTicketId))
                    {
                        reserveTickets.TryUpdate(rt.Key, reserveTicket, rt.Value);
                        Clients.Client(rt.Key).addMessage(reserveTicket);
                        continue;
                    }
                    Clients.Client(rt.Key).addMessage(reserveTicket);
                }
            }
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var reserveTicket = reserveTickets.FirstOrDefault(rt => rt.Key == Context.ConnectionId);
            if (reserveTicket.Value!=null)
            {
                new Thread(() =>
                {
                    var startBookingTime = Convert.ToDateTime(reserveTicket.Value.StartBooking);
                    var dateNow = DateTime.Now;
                    if (startBookingTime.CompareTo(dateNow) == 1)
                    {
                        Thread.Sleep(Convert.ToInt32(startBookingTime.Subtract(dateNow).TotalMilliseconds));
                    }                    
                    var temp = new ReserveTicketDto();
                    reserveTickets.TryRemove(Context.ConnectionId, out temp);
                    foreach (var item in reserveTickets)
                    {
                        var seatIds = reserveTickets.Where(rt => !rt.Value.GetId().Equals(item.Value.GetId())).SelectMany(rt => rt.Value.SeatIds).Distinct().ToList();
                        Clients.Client(item.Key).addMessage(new ReserveTicketDto {SeatIds = seatIds });
                    }
                    
                }).Start();
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}