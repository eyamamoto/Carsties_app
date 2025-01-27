'use client'

import {  Dropdown, DropdownDivider, DropdownItem } from 'flowbite-react'
import { User } from 'next-auth'
import { signOut } from 'next-auth/react'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import React from 'react'
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from 'react-icons/ai'
import { HiCog, HiUser } from 'react-icons/hi'
import { useParamsStore } from '../hooks/useParamsStore'

type Props = {
    user: User
}

export default function UserActions({user}:Props) {
    const router = useRouter();
    const pathname = usePathname();
    const setParams = useParamsStore(state => state.setParams);

    //quando altera com setparams, os parametros de busca são alterados no store, que faz todo o processo de atualização do state acontecer
    //realizando uma nova requisição com o parametro atualizado, set winner ou set seller
    function setWinner(){
        setParams({winner:user.username, seller:undefined})
        if(pathname !== "/") router.push("/")
    }

    function setSeller(){
        setParams({seller:user.username, winner:undefined})
        if(pathname !== "/") router.push("/")
    }

    return (
        <Dropdown inline label={`Welcome ${user.name}` }>
            <DropdownItem icon={HiUser} onClick={setSeller}>
                My Auctions
            </DropdownItem>
            <DropdownItem icon={AiFillTrophy} onClick={setWinner}>
                Auction won
            </DropdownItem>
            <DropdownItem icon={AiFillCar}>
                <Link href="/auctions/create">
                    Sell my car
                </Link>
            </DropdownItem>
            <DropdownItem icon={HiCog}>
                <Link href="/session">
                    Session (development only !)
                </Link>
            </DropdownItem>
            <DropdownDivider/>
            <DropdownItem icon={AiOutlineLogout} onClick={() => signOut({callbackUrl:'/'})}>
                Sign Out
            </DropdownItem>
        </Dropdown>
    )
}
