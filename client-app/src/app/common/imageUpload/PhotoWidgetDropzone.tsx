import React, {useCallback} from 'react'
import {useDropzone} from 'react-dropzone'
import { Header, Icon } from 'semantic-ui-react'

interface Props {
    setFiles : (files : any) => void; // can use type File too, but for ambiguity we use any
}

export default function PhotoWidgetDropzone({setFiles}: Props) {

    const dzStyles = {
        border: 'dashed 3px #eee',
        borderColor: '#eeee',
        borderRadius: '5px',
        paddingTop: '3em',
        textAlign: 'center' as 'center',
        height: 200,
    }

    const dzActive = {
        borderColor: 'green'
    }

    const onDrop = useCallback(acceptedFiles => {
        // Do something with the files
        setFiles(acceptedFiles.map((file: any) => Object.assign(file, {
            preview: URL.createObjectURL(file)
        })))
    }, [setFiles])
    const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

    return (
        <div {...getRootProps()} style={isDragActive ? {...dzStyles, ...dzActive} : dzStyles }>
            <input {...getInputProps()} />
                <Icon name='upload' size='huge' />
                <Header content='Drop image here' />
        </div>
    )
}
