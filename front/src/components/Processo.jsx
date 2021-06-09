import React from 'react'

export const Processo = () => {
    let [state, setState] = React.useState({ data: undefined })
    let [form, setForm] = React.useState({ })
    let [search, setSearch] = React.useState({ })
    let [responsaveis, setResponsaveis] = React.useState()
    let [processos, setProcessos] = React.useState()

    React.useEffect(() => {
        console.log('!!! REACT EFFECT !!!')
        console.log(state)
        if(responsaveis === undefined) {
            fetch(process.env.REACT_APP_BASE_URL + '/Responsavel/All?Size=999')
            .then(e => {
                if(e.ok) return e.json()
                throw new Error
            })
            .then(data => setResponsaveis(data))
            .catch(e => setResponsaveis(false))
        }

        if(processos === undefined) {
            fetch(process.env.REACT_APP_BASE_URL + '/Processo/All?Size=999')
            .then(e => {
                if(e.ok) return e.json()
                throw new Error
            })
            .then(data => setProcessos(data))
            .catch(e => setProcessos(false))
        }

        if(state.data === undefined) {
            fetch(process.env.REACT_APP_BASE_URL + '/Processo/All?'+ new URLSearchParams(search))
            .then(e => {
                if(e.ok) return e.json()
                throw new Error
            })
            .then(e => setState({...e}))
            .catch(e => setState({data : false}))
        }
    }, [state])

    const toggleModal = (modalState, modalAction, modalMethod) => {
        setState(s => ({...s, errors: undefined}));
        setForm(f => ({...f, action: modalAction, method: modalMethod, modal: modalState}))
    }

    const handleOnSubmit = (event) => {
        event.preventDefault()

        console.log(event.target.id)
        console.log(process.env.REACT_APP_BASE_URL + form.action)

        fetch('https://localhost:44302' + form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Id: +(event.target.id.value || 0),
                NumeroUnificado: event.target.numeroprocesso.value,
                DataDistribuicao: event.target.datadistribuicao.value || '0001-01-01',
                SegredoJustica: event.target.segredojustica.value,
                PastaFisicaCliente: event.target.pastafisicacliente.value,
                Descricao: event.target.descricao.value,
                SituacaoId: event.target.situacaoid.value,
                ProcessoPai: event.target.processoPai.value,
                Responsaveis: [...event.target['responsaveis[]'].options].filter(f => f.selected).map(opt => ({Id: opt.value}))
            })
        }).then(e =>{ 
            if(e.ok) return e.json()

            e.text()
            .then(text =>setState(s => ({...s, errors: JSON.parse(text).errors}) ))
            throw new Error
        })
        .then(e => {
            setState({data : undefined})
            setProcessos(undefined)
            setForm({})
            event.target.reset()
        })
        .catch(e => {
            setState(s => ({...s}))
        })
    }

    const handleOnSearch = (event) => {
        event.preventDefault()
        const data = [...event.target.elements].reduce((data, element) => {
            if (element.name && element.value) {
              data[element.name] = element.value;
            }
            return data;
        }, {});
        
        data.page = 1

        console.log(data)

        setSearch(data)
        setState({data : undefined})
    }

    const error = (name) => {
        return state && state.errors && Array.isArray(state.errors[name]) && state.errors[name].map(item =><p className="help is-danger">{item}</p>)
    }

    return (
        <div className="mt-5">
            {
                form.modal &&
                <div className={`modal ${form.modal && 'is-active'}`}>
                    <div className="modal-background"></div>
                    <div className="modal-content">
                        <div className="box">
                            <h2>Processo</h2>
                            <hr />
                            <form action={form.action} method={form.method} onSubmit={handleOnSubmit}>
                                {error('')}
                                <input name="id" type="hidden" defaultValue={form.id || 0} />
                                <div className="columns">
                                    <div className="column is-5">
                                        <div className="field">
                                            <label className="label">Número processo unificado</label>
                                            <input name="numeroprocesso" autoComplete="off" className="input" type="text" defaultValue={form.numeroUnificado} />
                                            {error('NumeroUnificado')}
                                        </div>
                                    </div>
                                    <div className="column is-3">
                                        <label className="label">Data distribuição</label>
                                        <div class="field">
                                            <p class="control">
                                                <input name="datadistribuicao" class="input" type="date" placeholder="00/00/0000" defaultValue={form.dataDistribuicao && form.dataDistribuicao.substring(0, 10)} />
                                            </p>
                                        </div>
                                        {error('DataDistribuicao')}
                                    </div>
                                    <div className="column">
                                        <div className="field">
                                            <label className="label">Situação</label>
                                            <div class="field">
                                                <p class="control">
                                                    <span class="select">
                                                    <select name="situacaoid" defaultValue={form.situacaoId}>
                                                        <option disabled hidden selected value="0">[ Selecione ]</option>
                                                        <option value="1">Arquivado</option>
                                                        <option value="2">Desmembrado</option>
                                                        <option value="3">Em andamento</option>
                                                        <option value="4">Em recurso</option>
                                                        <option value="5">Finalizado</option>
                                                    </select>
                                                    </span>
                                                </p>
                                            </div>
                                        {error('SituacaoId')}
                                        </div>
                                    </div>
                                    <div className="column is-2">
                                        <div class="field">
                                            <label className="label">Segredo de justiça</label>
                                            <div class="control mt-3">
                                                <label class="radio">
                                                    <input type="radio" name="segredojustica" value="true"  defaultChecked={form.segredoJustica} /> Sim
                                                </label>
                                                <label class="radio">
                                                    <input type="radio" name="segredojustica" value="false" defaultChecked={!form.segredoJustica} /> Não
                                                </label>
                                            </div>
                                        </div>
                                        {error('SegredoJustica')}
                                    </div>
                                </div>
                                <div className="columns">
                                    <div className="column is-5">
                                        <div class="field">
                                            <label className="label">Responsáveis</label>
                                            {error('Responsaveis')}
                                            <div class="select is-multiple" style={{width: '100%'}}>
                                                <select name="responsaveis[]" multiple size="6" style={{width: '100%'}}>
                                                    {
                                                        responsaveis && Array.isArray(responsaveis.data) && responsaveis.data.map(r => <option selected={form.responsaveis && [...form.responsaveis].some(x => x.responsavelId === r.id)} value={r.id}>{r.cpf} - {r.nome}</option>)
                                                    }
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="column is-7">
                                        <div className="field">
                                            <label className="label">Pasta física do cliente</label>
                                            <input name="pastafisicacliente" className="input" type="text" autoComplete="off" defaultValue={form.pastaFisicaCliente} ></input>
                                            {error('PastaFisicaCliente')}
                                        </div>
                                        
                                        <div className="field">
                                            <label className="label">Descrição</label>
                                            <textarea name="descricao" className="textarea" rows={5}>{form.descricao}</textarea>
                                        </div>
                                    </div>
                                </div>

                                <div className="columns">
                                    <div className="column is-12">
                                        <div className="field">
                                                <label className="label">Processo vinculado</label>
                                                <div class="field">
                                                    <p class="control">
                                                        <span class="select" style={{width: '100%'}}>
                                                        <select name="processoPai" defaultValue={form.processoPai} style={{width: '100%'}}>
                                                            <option disabled hidden selected value="">[ Escolha processo vinculado ]</option>
                                                            { processos && Array.isArray(processos.data) && processos.data.map(p => <option disabled={form.processoPai && form.id === p.id} selected={form.processoPai && form.processoPai === p.id} value={p.id}>{p.numeroUnificado} - {p.situacao.nome}</option>) }
                                                        </select>
                                                        </span>
                                                    </p>
                                                </div>
                                            {error('SituacaoId')}
                                        </div>
                                    </div>
                                </div>
                              
                                <div className="field is-grouped is-grouped-right">
                                    <p className="control">
                                        <button type="submit" className="button is-primary">Salvar</button>
                                    </p>
                                </div>
                            </form>
                        </div>
                    </div>
                    <button className="modal-close is-large" aria-label="close" onClick={ () => setForm({modal: false}) }></button>
                </div>
                
            }
            
            <div className="card">
                <div className="card-header">
                    <span className="card-header-title">Processo</span>
                    <button className="button is-small is-primary" onClick={ () => toggleModal(true, '/Processo/Create', 'POST') }>+ Novo processo</button>
                </div>
                <div className="card-content">
                    <div className="content">
                        <form name="search" action="" method="GET" onSubmit={handleOnSearch}>
                            <input name="page" className="input" type="hidden" value={search.page || 1} />
                            <fieldset>
                                <div className="columns">
                                    <div className="column is-3">
                                        <div className="field">
                                            <label className="label">Número processo unificado</label>
                                            <input name="NumeroUnificado" autoComplete="off" className="input masked" type="text" />
                                        </div>
                                    </div>
                                    <div className="column is-5">
                                            <label className="label">Data distribuição</label>
                                        <div class="field-body">
                                            <div class="field">
                                                <p class="control">
                                                    <input name="DataDistribuicaoInicio" class="input" type="date" placeholder="00/00/0000" />
                                                </p>
                                            </div>
                                            <div class="field">
                                                <p class="control">
                                                    <input name="DataDistribuicaoFim" class="input" type="date" placeholder="00/00/0000"  />
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="column">
                                        <div className="field">
                                            <label className="label">Situação</label>
                                            <div class="field">
                                                <p class="control">
                                                    <span class="select">
                                                        <select name="SituacaoId">
                                                            <option disabled hidden selected value="">[Filtrar por]</option>
                                                            <option value="">TODOS</option>
                                                            <option value="1">Arquivado</option>
                                                            <option value="2">Desmembrado</option>
                                                            <option value="3">Em andamento</option>
                                                            <option value="4">Em recurso</option>
                                                            <option value="5">Finalizado</option>
                                                        </select>
                                                    </span>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="column is-2">
                                        <div class="field">
                                            <label className="label">Segredo de justiça</label>
                                            <div class="field">
                                                <p class="control">
                                                    <span class="select">
                                                    <select name="SegredoJustica">
                                                        <option disabled hidden selected value="">[Filtrar por]</option>
                                                        <option value="">TODOS</option>
                                                        <option value="true">Sim</option>
                                                        <option value="false">Não</option>
                                                    </select>
                                                    </span>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="columns">
                                    <div className="column is-5">
                                        <div className="field">
                                            <label className="label">Pasta física do cliente</label>
                                            <input name="PastaFisicaCliente" autoComplete="off" className="input masked" type="text" />
                                        </div>
                                    </div>
                                    <div className="column is-5">
                                        <div className="field">
                                            <label className="label">Responsável</label>
                                            <input name="Responsavel" autoComplete="off" className="input masked" type="text" />
                                        </div>
                                    </div>
                                    <div className="column is-2">
                                        <div className="field is-grouped is-grouped-right mt-5">
                                            <button className="button is-primary" type="search">Consultar</button>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </form>


            {
                state.data === undefined &&
                <nav className="level mt-5">
                    <div className="level-item has-text-centered">
                        <div>
                        <p className="heading">Realizando consulta para...</p>
                        <p className="title">obter dados de processos!</p>
                        </div>
                    </div>
                </nav>
            }

            {
                state.data === false &&
                <nav className="level mt-5">
                    <div className="level-item has-text-centered">
                        <div>
                        <p className="heading">Foi realizado a consulta porém...</p>
                        <p className="title">ocorreu um erro!</p>
                        </div>
                    </div>
                </nav>
            }

            {
                state.data &&
                Array.isArray(state.data) && state.data.length === 0 &&
                <nav className="level mt-5">
                    <div className="level-item has-text-centered">
                        <div>
                        <p className="heading">Foi realizado a consulta porém...</p>
                        <p className="title">nenhum registro encontrado!</p>
                        </div>
                    </div>
                </nav>
            }

            {
                state.data &&
                Array.isArray(state.data) && state.data.length > 0 &&
                <>
                <table>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Data de distribuição</th>
                            <th>Pasta Física do Cliente</th>
                            <th>Segredo de Justiça</th>
                            <th>Responsáveis</th>
                            <th>Ação</th>
                        </tr>
                    </thead>
                    <tbody>
                           {
                                state.data.map(item =>
                                    <tr>
                                        <td>
                                            {item.numeroUnificado} ({item.situacao.nome})
                                            <br/>
                                            {item.descricao}
                                        </td>
                                        <td>{Intl.DateTimeFormat('ot-BR').format(new Date(item.dataDistribuicao))}</td>
                                        <td>{item.pastaFisicaCliente}</td>
                                        <td>
                                            {
                                                item.segredoJustica
                                                ? <span>Sim</span>
                                                : <span>Não</span>
                                            }
                                        </td>
                                        <td>{item.responsaveis.map(r => <span>{r.responsavel.nome}</span>)}</td>
                                        <td><button className="button is-small is-warning" onClick={() => { toggleModal(true, `/Processo/Update/${item.id}`, 'PATCH'); setForm(f => ({...f, ...item})); }}>Editar</button></td>
                                    </tr>
                                )
                            }
                    </tbody>
                </table>

                <nav className="pagination is-centered" role="navigation" aria-label="pagination">
                    <a className="pagination-previous" disabled={state && !state.previous} onClick={() => { setSearch(s => ({...s, page: state.previous})); setState({data: undefined}) }}>« Anterior</a>
                    <a className="pagination-next" disabled={state && !state.next} onClick={() => { setSearch(s => ({...s, page: state.next})); setState({data: undefined}) }}>Próximo »</a>
                    <ul className="pagination-list">
                        <li><a className="pagination-link is-current" aria-current="Página">Página {state.page}</a></li>
                    </ul>
                </nav>
                </>
            }
                    </div>
                </div>
            </div>
        </div>
    )
}