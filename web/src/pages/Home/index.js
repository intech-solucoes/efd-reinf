import React, { Component } from "react";
import { Box, Row, Col } from '../../components';

export default class Home extends Component {

    render() {
        return (
            <div>
                <Row>
                    <Col className="col-lg-8">
                        <Box titulo="Manuais para Geração do Arquivo CSV:" >
                            <h5>• <a href="layouts/Manual Geração CSV R-2010 - PrevSystem.pdf" download="Manual Geração CSV R-2010 - PrevSystem.pdf">PrevSystem</a></h5>
                            <h5>• <a href="layouts/Manual Geração CSV R-2010 - BrPrev.pdf" download="Manual Geração CSV R-2010 - BrPrev.pdf">BRPrev</a></h5>
                        </Box>
                    </Col>
                </Row>
                
                <Row>
                    <Col className="col-lg-8">
                        <Box>
                            <div align="center">
                                <img src="./imagens/EsquemaEFD-Reinf.png" alt="Esquema Intech EFD-Reinf" width="593" height="1048" />
                            </div>
                        </Box>
                    </Col>
                </Row>
            </div>
        );
    }

}