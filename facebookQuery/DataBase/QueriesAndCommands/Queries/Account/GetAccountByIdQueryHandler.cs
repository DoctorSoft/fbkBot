﻿using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, AccountModel>
    {
        private readonly DataBaseContext _context;

        public GetAccountByIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public AccountModel Handle(GetAccountByIdQuery query)
        {
            var models =
                _context.Accounts.Include(model => model.Cookies)
                    .Where(model => model.Id == query.UserId)
                    .Where(model => !model.IsDeleted || query.SearchDeleted)
                    .Select(model => new AccountModel
                    {
                        Id = model.Id,
                        PageUrl = model.PageUrl,
                        Login = model.Login,
                        Password = model.Password,
                        Cookie = model.Cookies!=null ? new CookieModel
                        {
                            CookieString = model.Cookies.CookiesString,
                            CreateDateTime = model.Cookies.CreateDate
                        } : null,
                        Name = model.Name,
                        FacebookId = model.FacebookId,
                        Proxy = model.Proxy,
                        ProxyLogin = model.ProxyLogin,
                        ProxyPassword = model.ProxyPassword,
                        GroupSettingsId = model.GroupSettingsId,
                        AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                        ProxyDataIsFailed = model.ProxyDataIsFailed,
                        ConformationIsFailed = model.ConformationIsFailed,
                        UserAgentId = model.UserAgentId,
                        IsDeleted = model.IsDeleted
                    }).FirstOrDefault();

            return models;
        }
    }
}
