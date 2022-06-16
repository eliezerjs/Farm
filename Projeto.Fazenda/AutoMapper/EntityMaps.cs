using AutoMapper;
using Projeto.Avaliacao.API.DTOs.Response;
using Projeto.Avaliacao.API.DTOs.Request;
using Projeto.Avaliacao.API.Models;
using System;

namespace Projeto.Avaliacao.API.AutoMapper
{
    public class FazendaParaFazendaResponseDto : Profile
    {
        public FazendaParaFazendaResponseDto()
        {
            CreateMap<Fazenda, FazendaResponseDto>(MemberList.None);
            CreateMap<FazendaResponseDto, Fazenda>(MemberList.None);
        }        
    }

    public class DispositivoParaDispositivoResponseDto : Profile
    {
        public DispositivoParaDispositivoResponseDto()
        {
            CreateMap<Dispositivo, DispositivoResponseDto>(MemberList.None);
            CreateMap<DispositivoResponseDto, Dispositivo>(MemberList.None);
        }
    }

    public class TelemetriaParaTelemetriaResponseDto : Profile
    {
        public TelemetriaParaTelemetriaResponseDto()
        {
            CreateMap<Telemetria, TelemetriaResponseDto>(MemberList.None);
            CreateMap<TelemetriaResponseDto, Telemetria>(MemberList.None);
        }
    }

    public class FazendaParaFazendaRequestDto : Profile
    {
        public FazendaParaFazendaRequestDto()
        {
            CreateMap<Fazenda, FazendaRequestDto>(MemberList.None);
            CreateMap<FazendaRequestDto, Fazenda>(MemberList.None);
        }
    }

    public class DispositivoParaDispositivoRequestDto : Profile
    {
        public DispositivoParaDispositivoRequestDto()
        {
            CreateMap<Dispositivo, DispositivoRequestDto>(MemberList.None);
            CreateMap<DispositivoRequestDto, Dispositivo>(MemberList.None);
        }
    }

    public class TelemetriaParaTelemetriaRequestDto : Profile
    {
        public TelemetriaParaTelemetriaRequestDto()
        {
            CreateMap<Telemetria, TelemetriaRequestDto>(MemberList.None);
            CreateMap<TelemetriaRequestDto, Telemetria>(MemberList.None);
        }
    }
}
