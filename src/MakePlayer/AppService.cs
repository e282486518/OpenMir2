﻿using MakePlayer.Cliens;
using MakePlayer.Option;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SystemModule;

namespace MakePlayer
{
    public class AppService : IHostedService
    {
        private readonly MakePlayOptions _options;
        private readonly ClientManager _clientManager;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private int _loginIndex;
        private int _loginTimeTick;

        public AppService(IOptions<MakePlayOptions> options, ClientManager clientManager)
        {
            _clientManager = clientManager;
            _options = options.Value;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _options.ChrCount = HUtil32._MIN(_options.ChrCount, _options.TotalChrCount);
            _loginTimeTick = HUtil32.GetTickCount() - 1000 * _options.ChrCount;
            Task.Factory.StartNew(Start, _cancellation.Token);
            _clientManager.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _clientManager.Stop();
            _cancellation.Cancel();
            return Task.CompletedTask;
        }
        
        void Start()
        {
            while (!_cancellation.IsCancellationRequested)
            {
                if (_options.TotalChrCount > 0)
                {
                    if ((HUtil32.GetTickCount() - _loginTimeTick) > 1000 * _options.ChrCount)
                    {
                        _loginTimeTick = HUtil32.GetTickCount();
                        if (_options.TotalChrCount >= _options.ChrCount)
                        {
                            _options.TotalChrCount -= _options.ChrCount;
                        }
                        else
                        {
                            _options.TotalChrCount = 0;
                        }
                        for (var i = 0; i < _options.ChrCount; i++)
                        {
                            var playClient = new PlayClient(_clientManager);
                            playClient.SessionId = Guid.NewGuid().ToString("N");
                            playClient.m_boNewAccount = _options.NewAccount;
                            playClient.m_sLoginAccount = string.Concat(_options.LoginAccount, _loginIndex);
                            playClient.m_sLoginPasswd = playClient.m_sLoginAccount;
                            playClient.m_sCharName = playClient.m_sLoginAccount;
                            playClient.m_sServerName = _options.ServerName;
                            //playClient.ClientSocket.Close();
                            playClient.ClientSocket.Host = _options.Address;
                            playClient.ClientSocket.Port = _options.Port;
                            playClient.m_dwConnectTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            _clientManager.AddClient(playClient.SessionId, playClient);
                            _loginIndex++;
                        }
                    }
                }
                _clientManager.Run();
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}