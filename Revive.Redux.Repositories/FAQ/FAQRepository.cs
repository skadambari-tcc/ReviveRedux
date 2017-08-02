using Revive.Redux.Common;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Revive.Redux.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private ILogService logService = null;

        public FAQRepository()
        {
            logService = new LogService();
        }

        public IEnumerable<FAQModel> GetFAQs(string pageAccessCode)
        {
            IEnumerable<FAQModel> faqList = new List<FAQModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    int grpId = 0;
                    if (pageAccessCode == PageAccessCode.SUPERADMIN || pageAccessCode == PageAccessCode.ADMIN || pageAccessCode == PageAccessCode.ACCNTMGR || pageAccessCode == PageAccessCode.CLIENTEXEC || pageAccessCode == PageAccessCode.APPROVER)
                    {
                        faqList = dbContext.FAQ_Management.AsEnumerable().Select(d => new FAQModel()
                        {
                            FAQRowID = d.FAQ_ID,
                            UserGrpId = d.Usergrp_Id,
                            UserGrpName = dbContext.UserTypes.FirstOrDefault(c => c.Id == d.Usergrp_Id).UserType1,
                            Question = HttpUtility.HtmlDecode(d.Question),
                            Answer = HttpUtility.HtmlDecode(d.Answer),
                            CreatedBy = d.Created_by,
                            CreatedDate = d.Created_Date,
                            ModifiedBy = d.Modified_by,
                            ModifiedDate = d.Modified_Date
                        }).ToList();
                    }
                    else if (pageAccessCode == PageAccessCode.MFPC || pageAccessCode == PageAccessCode.MFASSEMBLY || pageAccessCode == PageAccessCode.MFSHIP || pageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        if (pageAccessCode == PageAccessCode.CUSTOMERADMIN)
                            grpId = dbContext.UserTypes.Where(d => d.UserType1 == "Customer").FirstOrDefault().Id;
                        else
                            grpId = dbContext.UserTypes.Where(d => d.UserType1 == "Manufacturer").FirstOrDefault().Id;
                        faqList = dbContext.FAQ_Management.Where(c => c.Usergrp_Id == grpId).AsEnumerable().Select(d => new FAQModel()
                        {
                            FAQRowID = d.FAQ_ID,
                            UserGrpId = d.Usergrp_Id,
                            UserGrpName = dbContext.UserTypes.FirstOrDefault(c => c.Id == d.Usergrp_Id).UserType1,
                            Question = HttpUtility.HtmlDecode(d.Question),
                            Answer = HttpUtility.HtmlDecode(d.Answer),
                            CreatedBy = d.Created_by,
                            CreatedDate = d.Created_Date,
                            ModifiedBy = d.Modified_by,
                            ModifiedDate = d.Modified_Date
                        }).ToList();
                    }
                    if (faqList != null && faqList.Count() > 0)
                        faqList.OrderBy(f => f.FAQRowID);
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
            return faqList;
        }
        public FAQModel GetFAQById(int rowId)
        {
            var model = new FAQModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    model = (FAQModel)(from FAQ_Management row in dbContext.FAQ_Management
                                       where row.FAQ_ID == rowId
                                       select new FAQModel
                                        {
                                            FAQRowID = row.FAQ_ID,
                                            UserGrpId = row.Usergrp_Id,
                                            UserGrpName = dbContext.UserTypes.FirstOrDefault(c => c.Id == row.Usergrp_Id).UserType1,
                                            Question = HttpUtility.HtmlDecode(row.Question),
                                            Answer = HttpUtility.HtmlDecode(row.Answer),
                                            CreatedBy = row.Created_by,
                                            CreatedDate = row.Created_Date,
                                            ModifiedBy = row.Modified_by,
                                            ModifiedDate = row.Modified_Date
                                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return model;
        }

        public void InsertFAQs(List<FAQModel> faqModel, Guid userId)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    foreach (FAQModel current in faqModel)
                    {
                        string quest = current.Question.Replace("&lt;", "<").Replace("&gt;",">");
                        string answer = current.Answer.Replace("&lt;", "<").Replace("&gt;", ">");
                        FAQ_Management row = new FAQ_Management();
                        row.Question = HttpUtility.HtmlEncode(quest);
                        row.Answer = HttpUtility.HtmlEncode(answer);
                        row.Usergrp_Id = current.UserGrpId;
                        if (current.CreatedDate != null)
                        {
                            row.Created_by = current.CreatedBy;
                            row.Created_Date = current.CreatedDate;
                            row.Modified_by = userId;
                            row.Modified_Date = DateTime.Now;
                        }
                        else
                        {
                            row.Created_by = userId;
                            row.Created_Date = DateTime.Now;
                        }
                        dbContext.FAQ_Management.Add(row);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteFAQs()
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    dbContext.Database.ExecuteSqlCommand("Truncate table FAQ_Management");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
