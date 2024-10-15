export {auth as middleware} from '@/auth'

//protegendo rotas
export const config = {
    matcher:[
        '/session'
    ],
    pages:{
        signIn: "/api/auth/signin"
    },
    unstable_allowDynamic: [
        '/node_modules/@babel/runtime/regenerator/index.js',
        '/node_modules/next-auth/core/errors.js',
        '/node_modules/next-auth/utils/logger.js',
        '/node_modules/next-auth/core/index.js',
        '/node_modules/next-auth/next/index.js',
        '/node_modules/next-auth/index.js',
        '/auth.ts'
      ],
}
