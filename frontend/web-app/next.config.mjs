/** @type {import('next').NextConfig} */
const nextConfig = {
    //elhorando logs
    logging: {
        fetches:{
            fullUrl:true
        }
    },
    //imagens
    images:{
        remotePatterns:[
            {protocol:'https', hostname:'cdn.pixabay.com'}
        ]
    }

};

export default nextConfig;
