using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RehabCV.Extension;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RehabCV.Working
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private int number = 0;
        public Worker(ILogger<Worker> logger,
                      IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task CheckReserv(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var _events = scope.ServiceProvider.GetService<IEvent<Event>>();
                var _reserv = scope.ServiceProvider.GetService<IReserv<Reserve>>();
                var _rehabilitation = scope.ServiceProvider.GetService<IRehabilitation<Rehabilitation>>();
                var _queue = scope.ServiceProvider.GetService<IQueue<Queue>>();
                var _group = scope.ServiceProvider.GetService<IGroup<Group>>();
                var _child = scope.ServiceProvider.GetService<IRepository<Child>>();

                var dates = await _events.FindAll();

                foreach (var item in dates)
                {
                    if (DateTime.UtcNow.AddDays(7) >= item.Start)
                    {
                        Interlocked.Increment(ref number);
                        _logger.LogInformation($"Worker printing number: {number}");

                        var reserv = await _reserv.GetReserv();

                        if (reserv == null)
                        {
                            break;
                        }

                        var children = reserv.Children;

                        foreach (var child in children)
                        {
                            var rehab = await _rehabilitation.FindByChildId(child.Id);

                            var group = await _group.FindById(child.GroupId);

                            if (rehab.DateOfRehab == item.Start)
                            {
                                var queue = new Queue
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    RehabilitationId = rehab.Id,
                                    GroupOfDisease = group.NameOfDisease
                                };

                                if (await _group.AreSeats("Неврологія"))
                                {
                                    queue.GroupOfDisease = "Неврологія";
                                }
                                else if (await _group.AreSeats("Ортопедія"))
                                {
                                    queue.GroupOfDisease = "Ортопедія";
                                }
                                else if (await _group.AreSeats("Інше"))
                                {
                                    queue.GroupOfDisease = "Інше";
                                }
                                else if (await _group.AreSeats("Генетика"))
                                {
                                    queue.GroupOfDisease = "Генетика";
                                }
                                else
                                {
                                    queue.GroupOfDisease = "Психіатрія";
                                }

                                await _group.ChangeDisease(_child, child, child.Id, queue.GroupOfDisease);

                                await _queue.AddToQueue(queue);
                            }
                        }
                    }
                }

                
                await Task.Delay(TimeSpan.FromMinutes(145), cancellationToken);
            }   
        }
    }
}
