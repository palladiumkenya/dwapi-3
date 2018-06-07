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
    public class PatientArtLoader : IPatientArtLoader
    {
        private readonly IPatientArtExtractRepository _patientArtExtractRepository;
        private readonly ITempPatientArtExtractRepository _tempPatientArtExtractRepository;

        public PatientArtLoader(IPatientArtExtractRepository patientArtExtractRepository, ITempPatientArtExtractRepository tempPatientArtExtractRepository)
        {
            _patientArtExtractRepository = patientArtExtractRepository;
            _tempPatientArtExtractRepository = tempPatientArtExtractRepository;
        }

        public async Task<int> Load(int found)
        {
            try
            {
                DomainEvents.Dispatch(
                    new ExtractActivityNotification(new DwhProgress(
                        nameof(PatientArtExtract),
                        nameof(ExtractStatus.Loading),
                        found, 0, 0, 0, 0)));

                //load temp extracts without errors
                StringBuilder query = new StringBuilder();
                query.Append($" SELECT * FROM {nameof(TempPatientArtExtract)}s");
                query.Append($" INNER JOIN PatientExtracts p ON ");
                query.Append($" s.PatientPK = p.PatientPK AND ");
                query.Append($" s.SiteCode = p.SiteCode ");
                query.Append($" WHERE s.CheckError = 0");

                var tempPatientArtExtracts = _tempPatientArtExtractRepository.GetFromSql(query.ToString()).ToList();

                //Auto mapper
                var extractRecords = Mapper.Map<List<TempPatientArtExtract>, List<PatientArtExtract>>(tempPatientArtExtracts);

                //Batch Insert
                _patientArtExtractRepository.BatchInsert(extractRecords);
                Log.Debug("saved batch");


                return tempPatientArtExtracts.Count;

            }
            catch (Exception e)
            {
                Log.Error(e, $"Extract {nameof(PatientArtExtract)} not Loaded");
                return 0;
            }
        }
    }
}
