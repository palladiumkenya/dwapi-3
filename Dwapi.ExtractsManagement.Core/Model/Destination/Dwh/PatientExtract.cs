﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dwapi.SharedKernel.Model;
using Dwapi.SharedKernel.Utility;

namespace Dwapi.ExtractsManagement.Core.Model.Destination.Dwh
{
    public class PatientExtract : Entity<Guid>
    {
        [Key, Column(Order = 1)]
        public int PatientPK { get; set; }
        [Key, Column(Order = 2)]
        public int SiteCode { get; set; }
        public string PatientID { get; set; }
        [Column(Order = 100)]
        public string Emr { get; set; }
        [Column(Order = 101)]
        public string Project { get; set; }
        [DoNotRead]
        [Column(Order = 102)]
        public virtual bool? Processed { get; set; }

        [DoNotRead]
        public virtual string QueueId { get; set; }
        [DoNotRead]
        public virtual string Status { get; set; }
        [DoNotRead]
        public virtual DateTime? StatusDate { get; set; }
        
        public string FacilityName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? RegistrationAtCCC { get; set; }
        public DateTime? RegistrationATPMTCT { get; set; }
        public DateTime? RegistrationAtTBClinic { get; set; }
        public string PatientSource { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string ContactRelation { get; set; }
        public DateTime? LastVisit { get; set; }
        public string MaritalStatus { get; set; }
        public string EducationLevel { get; set; }
        public DateTime? DateConfirmedHIVPositive { get; set; }
        public string PreviousARTExposure { get; set; }
        public DateTime? DatePreviousARTStart { get; set; }
        public string StatusAtCCC { get; set; }
        public string StatusAtPMTCT { get; set; }
        public string StatusAtTBClinic { get; set; }

        //TODO add foreign Key relations

        public ICollection<PatientArtExtract> PatientArtExtracts { get; set; } = new List<PatientArtExtract>();
        public ICollection<PatientBaselinesExtract> PatientBaselinesExtracts { get; set; }=new List<PatientBaselinesExtract>();
        public ICollection<PatientLaboratoryExtract> PatientLaboratoryExtracts { get; set; }=new List<PatientLaboratoryExtract>();
        public ICollection<PatientPharmacyExtract> PatientPharmacyExtracts { get; set; }=new List<PatientPharmacyExtract>();
        public ICollection<PatientStatusExtract> PatientStatusExtracts { get; set; }=new List<PatientStatusExtract>();
        public ICollection<PatientVisitExtract> PatientVisitExtracts { get; set; }=new List<PatientVisitExtract>();
    }
}