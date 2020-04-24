﻿using System.Threading;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Commands.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators.Mgs;
using Dwapi.ExtractsManagement.Core.Model.Destination.Mgs;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Model;
using MediatR;

namespace Dwapi.ExtractsManagement.Core.ComandHandlers.Mgs
{
    public class ExtractMetricMigrationHandler :IRequestHandler<ExtractMetricMigration,bool>
    {
        private readonly IMetricMigrationSourceExtractor _patientSourceExtractor;
        private readonly IMetricExtractValidator _extractValidator;
        private readonly IMetricMigrationLoader _migrationLoader;
        private readonly ICleanMgsExtracts _cleanMgsExtracts;
        private readonly ITempMetricMigrationExtractRepository _tempMetricMigrationExtractRepository;
        private readonly IExtractHistoryRepository _extractHistoryRepository;

        public ExtractMetricMigrationHandler(IMetricMigrationSourceExtractor patientSourceExtractor, IMetricExtractValidator extractValidator, IMetricMigrationLoader migrationLoader, ICleanMgsExtracts cleanMgsExtracts, ITempMetricMigrationExtractRepository tempMetricMigrationExtractRepository, IExtractHistoryRepository extractHistoryRepository)
        {
            _patientSourceExtractor = patientSourceExtractor;
            _extractValidator = extractValidator;
            _migrationLoader = migrationLoader;
            _cleanMgsExtracts = cleanMgsExtracts;
            _tempMetricMigrationExtractRepository = tempMetricMigrationExtractRepository;
            _extractHistoryRepository = extractHistoryRepository;
        }

        public async Task<bool> Handle(ExtractMetricMigration request, CancellationToken cancellationToken)
        {

            //Extract
            int found = await _patientSourceExtractor.Extract(request.Extract, request.DatabaseProtocol);


            //Validate
            await _extractValidator.Validate(request.Extract.Id, found, "Migration", "TempMetricMigrationExtracts");

            //Load
            int loaded = await _migrationLoader.Load(request.Extract.Id, found);

            int rejected =
                _extractHistoryRepository.ProcessRejected(request.Extract.Id, found - loaded, request.Extract);


            _extractHistoryRepository.ProcessExcluded(request.Extract.Id, rejected,request.Extract);

            //notify loaded
            DomainEvents.Dispatch(
                new MgsExtractActivityNotification(request.Extract.Id, new ExtractProgress(
                    nameof(MetricMigrationExtract),
                    nameof(ExtractStatus.Loaded),
                    found, loaded, rejected, loaded, 0)));

            return true;
        }
    }
}