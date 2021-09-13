import React, {useCallback} from 'react'
import {useDropzone} from 'react-dropzone'

interface Props {
    setFiles : (files : any) => void; // can use type File too, but for ambiguity we use any
}

export default function PhotoWidgetDropzone({setFiles}: Props) {

    const dzStyles = {
        border: 'dashed 3px #eee',
        borderColor: '#eeee',
        borderRadius: '5px',
        paddingTop: '30 px',
        textAlign: 'center',
        height: 200,
    }

    const dzActive = {
        borderColor: 'green'
    }

    const onDrop = useCallback(acceptedFiles => {
        // Do something with the files
        console.log(acceptedFiles);
    }, [])
    const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

    return (
        <div {...getRootProps()}>
            <input {...getInputProps()} />
            {
                isDragActive ?
                <p>Drop the files here ...</p> :
                <p>Drag 'n' drop some files here, or click to select files</p>
            }
        </div>
    )
}
