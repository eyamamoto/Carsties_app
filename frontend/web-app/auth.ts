import NextAuth, { Profile } from "next-auth"
import { OIDCConfig } from "next-auth/providers"
import DuendeIDS6Provider from 'next-auth/providers/duende-identity-server6'

export const { handlers, signIn, signOut, auth } = NextAuth({
    session:{
        strategy:'jwt'
    },
    //providers de configuração do duende com auth.js
    providers: [
        DuendeIDS6Provider({
            id: 'id-server',
            clientId: 'nextApp',
            clientSecret: 'secret',
            issuer:'http://localhost:5000',
            authorization: {
                params: { scope: 'openid profile auctionApp' }
            },
            idToken:true
        } as OIDCConfig<Omit<Profile, 'username'>>),
    ],
    //recuperando dados de sessão do usuario
    callbacks: {
        //protegendo rotas
        async authorized({auth}){
            return !!auth
        },
        //recupera o token e as informações de sessão do usuario logado
        async jwt({token, profile, account}){

            if(account && account.access_token){
                token.accessToken = account.access_token
            }

            //console.log({token, user, account, profile})
            if(profile){
                token.username = profile.username
            }
            return token;
        },
        //sessão do usuario
        async session({session, token}){
            console.log({session, token})
            if(token){
                session.user.username = token.username;
                session.accessToken = token.accessToken
            }
            return session;
        }
    }
})

