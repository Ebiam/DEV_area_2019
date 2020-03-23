import React from 'react';
import sbooba from './sbooba.txt';
export default class GetApp extends React.Component {
    render() {
        return (
            <a href={sbooba} download>Download App</a>
        );
    }
}
