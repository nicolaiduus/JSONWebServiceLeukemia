using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

namespace JSONWebServiceLeukemia
{
    public class Service1 : IService1
    {
        public wsUploadResult SaveData(Stream JSONdataStream)
        {
            wsUploadResult result = new wsUploadResult();
            try
            {
                // Read in our Stream into a string...
                StreamReader reader = new StreamReader(JSONdataStream);
                string JSONdata = reader.ReadToEnd();

                // ..then convert the string into a single "wsLeukemiaData" record.
                JavaScriptSerializer jss = new JavaScriptSerializer();
                wsLeukemiaData leukemiaData = jss.Deserialize<wsLeukemiaData>(JSONdata);
                if (leukemiaData == null)
                {
                    // Error: Couldn't deserialize our JSON string into a "wsLeukemiaData" object.
                    result.WasSuccessful = 0;
                    result.Exception = "Unable to deserialize the JSON data.";
                    return result;
                }

                //Here add data to dataBase
                LeukemiaDBDataContext dc = new LeukemiaDBDataContext();

                //1. Add patientdata
                Patient newPatient = new Patient
                {
                    patientid = leukemiaData.patientID
                };

                var patientsResult = dc.Patients.Where(p => p.patientid == newPatient.patientid).FirstOrDefault();
                
                if(patientsResult == null){
                    dc.Patients.InsertOnSubmit(newPatient);
                    dc.SubmitChanges();
                }
               
                //2. Add paindata or update it, if it already exists
                foreach(wsPainData pd in leukemiaData.painData){
                    addPainData(dc, pd, newPatient.patientid);
                }

                //3. Add medicinedata or update it, if it already exists
                foreach (wsMedicineData md in leukemiaData.medicineData)
                {
                    addMedicineData(dc, md, newPatient.patientid);
                }

                //4. Add diarydata or update it, if it already exists
                foreach (wsDiaryData dd in leukemiaData.diaryData)
                {
                    addDiaryData(dc, dd, newPatient.patientid);
                }

                //Print result
                result.WasSuccessful = 1;
                result.Exception = leukemiaData.ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.WasSuccessful = 0;
                result.Exception = ex.Message;
                return result;
            }
        }
        
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        private void addPainData (LeukemiaDBDataContext dc, wsPainData pd, String patientId)
        {
            DateTime dateTime = DateTime.ParseExact(pd.date, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            Paindata newPainData = new Paindata()
            {
                id = pd.id,
                paracetamol = pd.paracetamol,
                morphinelevel = pd.morphinelevel,
                morphineunit = pd.morphineunit,
                painlevel = pd.painlevel,
                paintype = pd.paintype,
                date = dateTime,
                patientid = patientId
            };
            var painDatasResult = dc.Paindatas.Where(pdr => pdr.id == newPainData.id).FirstOrDefault();
            if (painDatasResult == null)
            {
                dc.Paindatas.InsertOnSubmit(newPainData);
            }
            else
            {
                painDatasResult.paracetamol = pd.paracetamol;
                painDatasResult.morphinelevel = pd.morphinelevel;
                painDatasResult.morphineunit = pd.morphineunit;
                painDatasResult.painlevel = pd.painlevel;
                painDatasResult.paintype = pd.paintype;
                painDatasResult.date = dateTime;
            }
            dc.SubmitChanges();
        }

        private void addMedicineData(LeukemiaDBDataContext dc, wsMedicineData md, String patientId)
        {
            DateTime dateTime = DateTime.ParseExact(md.date, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            Bloodsampledata newBloodSampleData = new Bloodsampledata()
            {
                neutrofile = md.bloodSample.neutrofile,
                other = md.bloodSample.other,
                alat = md.bloodSample.alat,
                crp = md.bloodSample.crp,
                hemoglobin = md.bloodSample.hemoglobin,
                leukocytes = md.bloodSample.leukocytes,
                thrombocytes = md.bloodSample.thrombocytes
            };

            dc.Bloodsampledatas.InsertOnSubmit(newBloodSampleData);
            dc.SubmitChanges();

            Medicinedata newMedicineData = new Medicinedata()
            {
                id = md.id,
                kemotreatment = md.kemoTreatment,
                mtx = md.mtx,
                mercaptopurin = md.mercaptopurin,
                date = dateTime,
                bloodsampleid = newBloodSampleData.bloodsampleid,
                patientid = patientId

            };
            var medicineDatasResult = dc.Medicinedatas.Where(mdr => mdr.id == newMedicineData.id).FirstOrDefault();
            if (medicineDatasResult == null)
            {
                dc.Medicinedatas.InsertOnSubmit(newMedicineData);
            }
            else
            {
                var bloodSampleDataResult = dc.Bloodsampledatas.Where(bsdr => bsdr.bloodsampleid == medicineDatasResult.bloodsampleid).FirstOrDefault();
                bloodSampleDataResult.neutrofile = md.bloodSample.neutrofile;
                bloodSampleDataResult.other = md.bloodSample.other;
                bloodSampleDataResult.alat = md.bloodSample.alat;
                bloodSampleDataResult.crp = md.bloodSample.crp;
                bloodSampleDataResult.hemoglobin = md.bloodSample.hemoglobin;
                bloodSampleDataResult.leukocytes = md.bloodSample.leukocytes;
                bloodSampleDataResult.thrombocytes = md.bloodSample.thrombocytes;

                medicineDatasResult.kemotreatment = md.kemoTreatment;
                medicineDatasResult.mtx = md.mtx;
                medicineDatasResult.mercaptopurin = md.mercaptopurin;
                medicineDatasResult.date = dateTime;
                medicineDatasResult.bloodsampleid = bloodSampleDataResult.bloodsampleid;

                dc.Bloodsampledatas.DeleteOnSubmit(newBloodSampleData);
            }
            dc.SubmitChanges();
        }

        private void addDiaryData(LeukemiaDBDataContext dc, wsDiaryData dd, String patientId)
        {
            DateTime dateTime = DateTime.ParseExact(dd.date, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            Diarydata newDiaryData = new Diarydata()
            {
                id = dd.id,
                protocoltreatmentday = dd.protocolTreatmentDay,
                notes = dd.notes,
                date = dateTime,
                weight = dd.weight,
                patientid = patientId
            };
            var diaryDatasResult = dc.Diarydatas.Where(ddr => ddr.id == newDiaryData.id).FirstOrDefault();
            if (diaryDatasResult == null)
            {
                dc.Diarydatas.InsertOnSubmit(newDiaryData);
            }
            else
            {
                diaryDatasResult.protocoltreatmentday = dd.protocolTreatmentDay;
                diaryDatasResult.notes = dd.notes;
                diaryDatasResult.date = dateTime;
                diaryDatasResult.weight = dd.weight;
            }
            dc.SubmitChanges();
        }
    }
}
