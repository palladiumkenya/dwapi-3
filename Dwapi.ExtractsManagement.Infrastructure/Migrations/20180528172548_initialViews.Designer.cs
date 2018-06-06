﻿// <auto-generated />
using Dwapi.ExtractsManagement.Infrastructure;
using Dwapi.SharedKernel.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Dwapi.ExtractsManagement.Infrastructure.Migrations
{
    [DbContext(typeof(ExtractsContext))]
    [Migration("20180528172548_initialViews")]
    partial class initialViews
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dwapi.Domain.Models.PatientArtExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("AgeARTStart");

                    b.Property<decimal?>("AgeEnrollment");

                    b.Property<decimal?>("AgeLastVisit");

                    b.Property<DateTime?>("DOB");

                    b.Property<decimal?>("Duration");

                    b.Property<string>("Emr");

                    b.Property<DateTime?>("ExitDate");

                    b.Property<string>("ExitReason");

                    b.Property<DateTime?>("ExpectedReturn");

                    b.Property<string>("Gender");

                    b.Property<DateTime?>("LastARTDate");

                    b.Property<string>("LastRegimen");

                    b.Property<string>("LastRegimenLine");

                    b.Property<DateTime?>("LastVisit");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<string>("PatientSource");

                    b.Property<string>("PreviousARTRegimen");

                    b.Property<DateTime?>("PreviousARTStartDate");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("Provider");

                    b.Property<string>("QueueId");

                    b.Property<DateTime?>("RegistrationDate");

                    b.Property<int>("SiteCode");

                    b.Property<DateTime?>("StartARTAtThisFacility");

                    b.Property<DateTime?>("StartARTDate");

                    b.Property<string>("StartRegimen");

                    b.Property<string>("StartRegimenLine");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientArtExtract");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientBaselinesExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Emr");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("QueueId");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<int?>("bCD4");

                    b.Property<DateTime?>("bCD4Date");

                    b.Property<int?>("bWAB");

                    b.Property<DateTime?>("bWABDate");

                    b.Property<int?>("bWHO");

                    b.Property<DateTime?>("bWHODate");

                    b.Property<int?>("eCD4");

                    b.Property<DateTime?>("eCD4Date");

                    b.Property<int?>("eWAB");

                    b.Property<DateTime?>("eWABDate");

                    b.Property<int?>("eWHO");

                    b.Property<DateTime?>("eWHODate");

                    b.Property<int?>("lastCD4");

                    b.Property<DateTime?>("lastCD4Date");

                    b.Property<int?>("lastWAB");

                    b.Property<DateTime?>("lastWABDate");

                    b.Property<int?>("lastWHO");

                    b.Property<DateTime?>("lastWHODate");

                    b.Property<int?>("m12CD4");

                    b.Property<DateTime?>("m12CD4Date");

                    b.Property<int?>("m6CD4");

                    b.Property<DateTime?>("m6CD4Date");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientBaselinesExtract");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientLaboratoryExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Emr");

                    b.Property<int?>("EnrollmentTest");

                    b.Property<DateTime?>("OrderedByDate");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("QueueId");

                    b.Property<DateTime?>("ReportedByDate");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<string>("TestName");

                    b.Property<string>("TestResult");

                    b.Property<int?>("VisitId");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientLaboratoryExtract");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientPharmacyExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DispenseDate");

                    b.Property<string>("Drug");

                    b.Property<decimal?>("Duration");

                    b.Property<string>("Emr");

                    b.Property<DateTime?>("ExpectedReturn");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<string>("PeriodTaken");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("ProphylaxisType");

                    b.Property<string>("Provider");

                    b.Property<string>("QueueId");

                    b.Property<string>("RegimenLine");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<string>("TreatmentType");

                    b.Property<int?>("VisitID");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientPharmacyExtract");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientStatusExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Emr");

                    b.Property<DateTime?>("ExitDate");

                    b.Property<string>("ExitDescription");

                    b.Property<string>("ExitReason");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("QueueId");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientStatusExtract");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientVisitExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adherence");

                    b.Property<string>("AdherenceCategory");

                    b.Property<string>("BP");

                    b.Property<DateTime?>("EDD");

                    b.Property<string>("Emr");

                    b.Property<string>("FamilyPlanningMethod");

                    b.Property<decimal?>("GestationAge");

                    b.Property<decimal?>("Height");

                    b.Property<DateTime?>("LMP");

                    b.Property<DateTime?>("NextAppointmentDate");

                    b.Property<string>("OI");

                    b.Property<DateTime?>("OIDate");

                    b.Property<Guid?>("PatientExtractId");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<string>("Pregnant");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("PwP");

                    b.Property<string>("QueueId");

                    b.Property<DateTime?>("SecondlineRegimenChangeDate");

                    b.Property<string>("SecondlineRegimenChangeReason");

                    b.Property<string>("Service");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<DateTime?>("SubstitutionFirstlineRegimenDate");

                    b.Property<string>("SubstitutionFirstlineRegimenReason");

                    b.Property<DateTime?>("SubstitutionSecondlineRegimenDate");

                    b.Property<string>("SubstitutionSecondlineRegimenReason");

                    b.Property<DateTime?>("VisitDate");

                    b.Property<int?>("VisitId");

                    b.Property<string>("VisitType");

                    b.Property<string>("WABStage");

                    b.Property<int?>("WHOStage");

                    b.Property<decimal?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("PatientExtractId");

                    b.ToTable("PatientVisitExtract");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.ExtractHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ExtractId");

                    b.Property<int?>("Stats");

                    b.Property<int>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<string>("StatusInfo");

                    b.HasKey("Id");

                    b.ToTable("ExtractHistory");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.PsmartStage", b =>
                {
                    b.Property<Guid>("EId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateExtracted");

                    b.Property<DateTime?>("DateSent");

                    b.Property<DateTime>("DateStaged");

                    b.Property<DateTime?>("Date_Created");

                    b.Property<string>("Emr");

                    b.Property<int?>("Id");

                    b.Property<string>("RequestId");

                    b.Property<string>("Shr");

                    b.Property<string>("Status")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("Status_Date");

                    b.Property<string>("Uuid");

                    b.HasKey("EId");

                    b.ToTable("PsmartStage");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactRelation");

                    b.Property<DateTime?>("DOB");

                    b.Property<DateTime?>("DateConfirmedHIVPositive");

                    b.Property<DateTime?>("DatePreviousARTStart");

                    b.Property<string>("District");

                    b.Property<string>("EducationLevel");

                    b.Property<string>("Emr");

                    b.Property<string>("FacilityName");

                    b.Property<string>("Gender");

                    b.Property<DateTime?>("LastVisit");

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("PatientID");

                    b.Property<int>("PatientPK");

                    b.Property<string>("PatientSource");

                    b.Property<string>("PreviousARTExposure");

                    b.Property<bool?>("Processed");

                    b.Property<string>("Project");

                    b.Property<string>("QueueId");

                    b.Property<string>("Region");

                    b.Property<DateTime?>("RegistrationATPMTCT");

                    b.Property<DateTime?>("RegistrationAtCCC");

                    b.Property<DateTime?>("RegistrationAtTBClinic");

                    b.Property<DateTime?>("RegistrationDate");

                    b.Property<int>("SiteCode");

                    b.Property<string>("Status");

                    b.Property<string>("StatusAtCCC");

                    b.Property<string>("StatusAtPMTCT");

                    b.Property<string>("StatusAtTBClinic");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<string>("Village");

                    b.HasKey("Id");

                    b.ToTable("PatientExtracts");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.TempPatientExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CheckError");

                    b.Property<string>("ContactRelation");

                    b.Property<DateTime?>("DOB");

                    b.Property<DateTime?>("DateConfirmedHIVPositive");

                    b.Property<DateTime>("DateExtracted");

                    b.Property<string>("District");

                    b.Property<string>("EMR");

                    b.Property<string>("EducationLevel");

                    b.Property<int?>("FacilityId");

                    b.Property<string>("FacilityName");

                    b.Property<string>("Gender");

                    b.Property<DateTime?>("LastVisit");

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("PatientID");

                    b.Property<int?>("PatientPK");

                    b.Property<string>("PatientSource");

                    b.Property<string>("PreviousARTExposure");

                    b.Property<DateTime?>("PreviousARTStartDate");

                    b.Property<string>("Project");

                    b.Property<string>("Region");

                    b.Property<DateTime?>("RegistrationAtCCC");

                    b.Property<DateTime?>("RegistrationAtPMTCT");

                    b.Property<DateTime?>("RegistrationAtTBClinic");

                    b.Property<DateTime?>("RegistrationDate");

                    b.Property<string>("SatelliteName");

                    b.Property<int?>("SiteCode");

                    b.Property<string>("StatusAtCCC");

                    b.Property<string>("StatusAtPMTCT");

                    b.Property<string>("StatusAtTBClinic");

                    b.Property<string>("Village");

                    b.HasKey("Id");

                    b.ToTable("TempPatientExtracts");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.TempPatientExtractError", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CheckError");

                    b.Property<string>("ContactRelation");

                    b.Property<DateTime?>("DOB");

                    b.Property<DateTime?>("DateConfirmedHIVPositive");

                    b.Property<DateTime>("DateExtracted");

                    b.Property<string>("District");

                    b.Property<string>("EducationLevel");

                    b.Property<string>("Emr");

                    b.Property<int?>("FacilityId");

                    b.Property<string>("FacilityName");

                    b.Property<string>("Gender");

                    b.Property<DateTime?>("LastVisit");

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("PatientID");

                    b.Property<int?>("PatientPK");

                    b.Property<string>("PatientSource");

                    b.Property<string>("PreviousARTExposure");

                    b.Property<DateTime?>("PreviousARTStartDate");

                    b.Property<string>("Project");

                    b.Property<string>("Region");

                    b.Property<DateTime?>("RegistrationATPMTCT");

                    b.Property<DateTime?>("RegistrationAtCCC");

                    b.Property<DateTime?>("RegistrationAtTBClinic");

                    b.Property<DateTime?>("RegistrationDate");

                    b.Property<string>("SatelliteName");

                    b.Property<int?>("SiteCode");

                    b.Property<string>("StatusAtCCC");

                    b.Property<string>("StatusAtPMTCT");

                    b.Property<string>("StatusAtTBClinic");

                    b.Property<string>("Village");

                    b.HasKey("Id");

                    b.ToTable("vTempPatientExtractError");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.TempPatientExtractErrorSummary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateGenerated");

                    b.Property<string>("Extract");

                    b.Property<int?>("FacilityId");

                    b.Property<string>("FacilityName");

                    b.Property<string>("Field");

                    b.Property<string>("PatientID");

                    b.Property<int?>("PatientPK");

                    b.Property<Guid>("RecordId");

                    b.Property<int?>("SiteCode");

                    b.Property<string>("Summary");

                    b.Property<Guid?>("TempPatientExtractErrorId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("TempPatientExtractErrorId");

                    b.ToTable("vTempPatientExtractErrorSummary");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.ValidationError", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateGenerated");

                    b.Property<Guid>("RecordId");

                    b.Property<Guid>("ValidatorId");

                    b.HasKey("Id");

                    b.HasIndex("ValidatorId");

                    b.ToTable("ValidationError");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Validator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Extract");

                    b.Property<string>("Field");

                    b.Property<string>("Logic");

                    b.Property<string>("Summary");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Validator");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientArtExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientArtExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientBaselinesExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientBaselinesExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientLaboratoryExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientLaboratoryExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientPharmacyExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientPharmacyExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientStatusExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientStatusExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.Domain.Models.PatientVisitExtract", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.PatientExtract")
                        .WithMany("ClientPatientVisitExtracts")
                        .HasForeignKey("PatientExtractId");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.TempPatientExtractErrorSummary", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Source.Dwh.TempPatientExtractError")
                        .WithMany("TempPatientExtractErrorSummaries")
                        .HasForeignKey("TempPatientExtractErrorId");
                });

            modelBuilder.Entity("Dwapi.ExtractsManagement.Core.Model.ValidationError", b =>
                {
                    b.HasOne("Dwapi.ExtractsManagement.Core.Model.Validator")
                        .WithMany("ValidationErrors")
                        .HasForeignKey("ValidatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
