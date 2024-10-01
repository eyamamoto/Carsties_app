import React from 'react'

//carregar dados
async function getData(){
    const res = await fetch('http://localhost:6001/search');

    if(!res.ok) throw new Error('Fail to fetch error');

    return res.json();
}

export default async function Listings() {

    const data = await getData();

    return (
        <div>
            {JSON.stringify(data,null,2)}
        </div>
    )
}
