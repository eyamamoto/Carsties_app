export {auth as middleware} from '@/auth'

//protegendo rotas
export const config = {
    matcher:[
        '/session'
    ],
    pages:{
        signIn: "/api/auth/signin"
    }
}