"use client"

import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'
import { useParamsStore } from '../hooks/useParamsStore';
import { useRouter } from 'next/navigation';
import { usePathname } from 'next/navigation';


export default function Logo() {
    const reset = useParamsStore(state => state.reset);
    const router = useRouter();
    const pathname = usePathname();

    function doReset(){
        if(pathname !== "/") router.push("/");
        reset();
    }

    return (
        <div onClick={doReset} className='flex items-center gap-2 text-3xl font-semibold text-red-500'>
            <AiOutlineCar size={34}/>
            <div>Leilões</div>
        </div>
    )
}
