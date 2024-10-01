//renderizado no cliente
'use client'

import Image from 'next/image'
import React, { Fragment, useState } from 'react'

//props
type Props = {
    auction:any
}

export default function CarImage({auction}: Props) {
    const [isLoading, setLoading] = useState(true);
  return (
    <Fragment>
        <Image
                src={auction.imageUrl}
                alt={`Image of ${auction.make} ${auction.model} in ${auction.color}`}
                fill
                priority
                className={`
                        object-cover group-hover:opacity-75 duration-700 ease-in-out
                        ${isLoading ? 'greyscale blur-2xl scale-110' : 'grayscale-0 blur-0 scale-100'}
                    `}
                sizes='(max-width: 768px) 100vw, (max-width:1200px) 50vw, 25vw'
                onLoad={() => setLoading(false)}
            />
    </Fragment>
  )
}
