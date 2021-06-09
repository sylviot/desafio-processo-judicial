import React from 'react'

export const Responsavel = () => {
    let [state, setState] = React.useState({ data: undefined })
    let [form, setForm] = React.useState({ })
    let [search, setSearch] = React.useState({ })

    React.useEffect(() => {
        console.log('!!! REACT EFFECT !!!')
        if(!state.data) {
            fetch(process.env.REACT_APP_BASE_URL + '/Responsavel/All?'+ new URLSearchParams(search))
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
        console.log('onSubmit', form)

        fetch(process.env.REACT_APP_BASE_URL + form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Id: event.target.id.value,
                Cpf: event.target.cpf.value,
                Nome: event.target.nome.value,
                Email: event.target.email.value,
                Foto: event.target.foto.value,
            })
        }).then(e =>{ 
            if(e.ok) return e.json()

            e.text()
            .then(text =>setState(s => ({...s, errors: JSON.parse(text).errors}) ))
            throw new Error
        })
        .then(e => {
            setState({data : undefined})
            setForm({})
            event.target.reset()
        })
        .catch(e => {
            setState(s => ({...s, errors: { '': 'Não foi possível realizar a ação!'}}))
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

        setSearch(data)
        setState({data : undefined})
    }

    const error = (name) => {
        return state && state.errors && Array.isArray(state.errors[name]) && state.errors[name].map(item =><p class="help is-danger">{item}</p>)
    }

    return (
        <div className="mt-5">
            {
                form.modal &&
                <div className={`modal ${form.modal && 'is-active'}`}>
                    <div className="modal-background"></div>
                    <div className="modal-content">
                        <div className="box">
                            <h2>Responsável</h2>
                            <hr />
                            <form action={form.action} method={form.method} onSubmit={handleOnSubmit}>
                                {error('')}

                                <input name="id" className="input" type="hidden" defaultValue={form.id || 0} />
                                <div className="field">
                                    <label className="label">CPF</label>
                                    <input name="cpf" className="input" type="text" autoComplete="off" defaultValue={form.cpf} ></input>
                                    {error('Cpf')}
                                </div>
                                <div className="field">
                                    <label className="label">Nome</label>
                                    <input name="nome" className="input" type="text" autoComplete="off" defaultValue={form.nome} ></input>
                                    {error('Nome')}
                                </div>
                                <div className="field">
                                    <label className="label">E-mail</label>
                                    <input name="email" className="input" type="text" autoComplete="off" defaultValue={form.email} ></input>
                                    {error('Email')}
                                </div>
                                <div className="field is-horizontal">
                                    <label className="label">Foto</label>
                                    <input name="foto" className="input" type="text" autoComplete="off" defaultValue={form.foto} ></input>
                                    {error('Foto')}
                                </div>

                                <div class="field is-grouped is-grouped-right">
                                    <p class="control">
                                        <button type="submit" class="button is-primary">Salvar</button>
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
                    <span className="card-header-title">Responsável</span>
                    <button className="button is-small is-primary" onClick={ () => toggleModal(true, '/Responsavel/Create', 'POST') }>+ Novo responsável</button>
                </div>
                <div className="card-content">
                    <div className="content">
                        <form name="search" action="" method="GET" onSubmit={handleOnSearch}>
                            <input name="page" className="input" type="hidden" value={search.page || 1} />
                            <fieldset>
                                <div className="columns">
                                    <div className="column">
                                        <div className="field">
                                            <label className="label">CPF</label>
                                            <input name="Cpf" autoComplete="off" className="input" type="text" data-mask="999.999" />
                                        </div>
                                    </div>
                                    <div className="column">
                                        <div className="field">
                                            <label className="label">Nome</label>
                                            <input name="Nome" autoComplete="off" className="input" type="text" />
                                        </div>
                                    </div>
                                    <div className="column">
                                        <div className="field">
                                            <label className="label">Número Unificado do processo</label>
                                            <input name="NumeroProcessoUnificado" autoComplete="off" className="input" type="text" />
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
                        <p className="title">obter dados de responsáveis!</p>
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
                            <th>Nome</th>
                            <th>Ação</th>
                        </tr>
                    </thead>
                    <tbody>
                           {
                                state.data.map(item =>
                                    <tr>
                                        <td>{item.cpf}</td>
                                        <td>{item.nome}</td>
                                        <td><button className="button is-small is-warning" onClick={() => { toggleModal(true, `/Responsavel/Update/${item.id}`, 'PATCH'); setForm(f => ({...f, ...item})); }}>Editar</button></td>
                                    </tr>
                                )
                            }
                    </tbody>
                </table>

                <nav class="pagination is-centered" role="navigation" aria-label="pagination">
                    <a class="pagination-previous" disabled={state && !state.previous} onClick={() => { setSearch(s => ({...s, page: state.previous})); setState({data: undefined}) }}>« Anterior</a>
                    <a class="pagination-next" disabled={state && !state.next} onClick={() => { setSearch(s => ({...s, page: state.next})); setState({data: undefined}) }}>Próximo »</a>
                    <ul class="pagination-list">
                        <li><a class="pagination-link is-current" aria-current="Página">Página {state.page}</a></li>
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