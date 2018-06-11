﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Dwh;
using Dwapi.ExtractsManagement.Core.Model.Destination.Dwh;
using Dwapi.ExtractsManagement.Core.Model.Source.Dwh;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Model;
using Serilog;

namespace Dwapi.ExtractsManagement.Core.Loader.Dwh
{
    public class PatientVisitLoader : IPatientVisitLoader
    {
        private readonly IPatientVisitExtractRepository _patientVisitExtractRepository;
        private readonly ITempPatientVisitExtractRepository _tempPatientVisitExtractRepository;

        public PatientVisitLoader(IPatientVisitExtractRepository patientVisitExtractRepository, ITempPatientVisitExtractRepository tempPatientVisitExtractRepository)
        {
            _patientVisitExtractRepository = patientVisitExtractRepository;
            _tempPatientVisitExtractRepository = tempPatientVisitExtractRepository;
        }

        public async Task<int> Load(Guid extractId, int found)
        {
            try
            {
                DomainEvents.Dispatch(
                    new ExtractActivityNotification(extractId, new DwhProgress(
                        nameof(PatientVisitExtract),
                        nameof(ExtractStatus.Loading),
                        found, 0, 0, 0, 0)));

                //load temp extracts without errors
                StringBuilder query = new StringBuilder();
                query.Append($" SELECT * FROM {nameof(TempPatientVisitExtract)}s s");
                query.Append($" INNER JOIN PatientExtracts p ON ");
                query.Append($" s.PatientPK = p.PatientPK AND ");
                query.Append($" s.SiteCode = p.SiteCode ");
                query.Append($" WHERE s.CheckError = 0");

                var tempPatientVisitExtracts = await _tempPatientVisitExtractRepository.GetFromSql(query.ToString());

                //Auto mapper
                var extractRecords = Mapper.Map<List<TempPatientVisitExtract>, List<PatientVisitExtract>>(tempPatientVisitExtracts);

                //Batch Insert
                _patientVisitExtractRepository.BatchInsert(extractRecords);
                Log.Debug("saved batch");


                return tempPatientVisitExtracts.Count;

            }
            catch (Exception e)
            {
                Log.Error(e, $"Extract {nameof(PatientVisitExtract)} not Loaded");
                return 0;
            }
        }
    }
}