﻿using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Dwh;
using Dwapi.ExtractsManagement.Core.Model.Destination.Dwh;
using Dwapi.ExtractsManagement.Core.Model.Source.Dwh;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Model;
using Dwapi.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.ExtractsManagement.Core.Extractors.Dwh
{
    public class PatientBaselinesSourceExtractor : IPatientBaselinesSourceExtractor
    {
        private readonly IExtractSourceReader _reader;
        private readonly IMediator _mediator;
        private readonly ITempPatientBaselinesExtractRepository _extractRepository;

        public PatientBaselinesSourceExtractor(IExtractSourceReader reader, IMediator mediator, ITempPatientBaselinesExtractRepository extractRepository)
        {
            _reader = reader;
            _mediator = mediator;
            _extractRepository = extractRepository;
        }

        public async Task<int> Extract(DbExtract extract, DbProtocol dbProtocol)
        {
            int batch = 500;

            var list = new List<TempPatientBaselinesExtract>();

            int count = 0;

            using (var rdr = await _reader.ExecuteReader(dbProtocol, extract))
            {
                while (rdr.Read())
                {
                    count++;        
                    // AutoMapper profiles
                    var extractRecord = Mapper.Map<IDataRecord, TempPatientBaselinesExtract>(rdr);
                    extractRecord.Id = LiveGuid.NewGuid();
                    list.Add(extractRecord);

                    if (count == batch)
                    {
                        _extractRepository.BatchInsert(list);

                        count = 0;
                   

                        DomainEvents.Dispatch(
                            new ExtractActivityNotification(new DwhProgress(
                                nameof(PatientBaselinesExtract),
                                nameof(ExtractStatus.Finding),
                                list.Count, 0, 0, 0, 0)));
                        list = new List<TempPatientBaselinesExtract>();
                    }
                }

                if (count > 0)
                {
                    // save remaining list;
                    _extractRepository.BatchInsert(list);
                }
                _extractRepository.CloseConnection();
            }

            // TODO: Notify Completed;
            DomainEvents.Dispatch(
                new ExtractActivityNotification(extract.Id, new DwhProgress(
                    nameof(PatientBaselinesExtract),
                    nameof(ExtractStatus.Found),
                    list.Count, 0, 0, 0, 0)));

            return list.Count;
        }
    }
}