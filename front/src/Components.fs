namespace App

open Feliz
open Feliz.Router
open Feliz.Bulma

type Components =
    [<ReactComponent(import="Responsavel", from="./components/Responsavel.jsx")>]
    static member Responsavel() = React.imported()

    [<ReactComponent(import="Processo", from="./components/Processo.jsx")>]
    static member Processo() = React.imported()


    static member Layout(content : ReactElement) = 
        Html.div [
            Bulma.navbar [
                Bulma.color.isInfo
                prop.children [
                    Bulma.navbarBrand.div [
                        Bulma.navbarItem.a [
                            Html.img [ prop.src "https://bulma.io/images/bulma-logo-white.png"; prop.height 28; prop.width 112; ]
                        ]
                    ]
                    Bulma.navbarMenu [
                        Bulma.navbarStart.div [
                            Bulma.navbarItem.a [ prop.text "Página Inicial"; prop.href (Router.format "/") ]
                            Bulma.navbarItem.a [ prop.text "Responsável"; prop.href (Router.format "responsavel") ]
                            Bulma.navbarItem.a [ prop.text "Processo"; prop.href (Router.format "processo") ]
                        ]
                        Bulma.navbarEnd.div [
                            Bulma.navbarItem.div [
                                Bulma.media [
                                    Bulma.mediaContent [
                                        Bulma.text.hasTextRight
                                        prop.children [
                                            Bulma.title.p [
                                                Bulma.text.hasTextWeightBold
                                                Bulma.title.is6
                                                prop.text "Administrador"
                                            ]
                                            Bulma.subtitle.p [
                                                Bulma.title.is6
                                                Bulma.text.hasTextWeightLight
                                                Bulma.color.hasTextWhite
                                                prop.text "@admin"
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            Bulma.container [ 
                Bulma.content [ content ]
             ]
        ]

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Home() =
        Html.div [
           Bulma.hero [
               Bulma.heroBody [
                   Html.p [
                       prop.className "title"
                       prop.text "Gerenciamento de processos judiciais"
                   ]
                   Bulma.columns [
                       Bulma.column [
                           Html.p [
                               prop.className "subtitle"
                               prop.text "Backend"
                           ]
                           Html.ul [
                               Html.li "WebApiCore .net5"
                               Html.li "Auto Mapper"
                               Html.li "Entity Core"
                               Html.li "Fluent Validation"
                               Html.li "Hangfire"
                               Html.li "Serilog"
                               Html.li "Swagger"
                           ]

                           Html.p [
                               prop.className "subtitle"
                               prop.text "Database"
                           ]
                           Html.ul [
                               Html.li "MSSql Server"
                           ]
                       ]
                       Bulma.column [
                           Html.p [
                               prop.className "subtitle"
                               prop.text "Frontend"
                           ]
                           Html.ul [
                               Html.li "Fable"
                               Html.li "Feliz"
                               Html.li "ReactJS"
                               Html.li "Bulma CSS"
                           ]
                       ]
                       Bulma.column [
                           Html.p [
                               prop.className "subtitle"
                               prop.text "Infra"
                           ]
                           Html.ul [
                               Html.li "Docker"
                               Html.li "Docker Compose"
                               Html.li "Github"
                           ]
                       ]
                       Bulma.column [
                           Html.p [
                               prop.className "subtitle"
                               prop.text "Linguagens"
                           ]
                           Html.ul [
                               Html.li "CSharp (C#)"
                               Html.li "Javscript"
                               Html.li "SQL"
                               Html.li "FSharp (F#)"
                           ]
                       ]
                       
                   ]


               ]
           ]
        ]

    /// <summary>
    /// A React component that uses Feliz.Router
    /// to determine what to show based on the current URL
    /// </summary>
    [<ReactComponent>]
    static member Router() =
        let (currentUrl, updateUrl) = React.useState(Router.currentUrl())
        React.router [
            router.onUrlChanged updateUrl
            router.children [
                match currentUrl with
                | [ ] -> Components.Home() |> Components.Layout
                | [ "responsavel" ] -> Components.Responsavel() |> Components.Layout
                | [ "processo" ] -> Components.Processo() |> Components.Layout
                | otherwise -> Html.h1 "Not found"
            ]
        ]