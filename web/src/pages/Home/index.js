import React, { Component } from "react";
import { Box, Row, Col } from '../../components';
import { Page } from "../";

export default class Home extends Component {

    constructor(props) {
        super(props);

        this.page = React.createRef();
    }

    async componentDidMount() {
        this.page.current.loading(false);
    }

    render() {
        return (
            <Page {...this.props} ref={this.page}>
                <Row>
                    <Col>
                        <Box titulo="Manuais para Geração do Arquivo CSV:" >
                            <h5>• <a href="layouts/Manual Geração CSV R-2010 - PrevSystem.pdf" download="Manual Geração CSV R-2010 - PrevSystem.pdf">PrevSystem</a></h5>
                            <h5>• <a href="layouts/Manual Geração CSV R-2010 - BrPrev.pdf" download="Manual Geração CSV R-2010 - BrPrev.pdf">BRPrev</a></h5>
                        </Box>
                    </Col>
                </Row>
                
                <Row>
                    <Col>
                        <Box titulo="Faça aqui o download do transmissor:">
                            <h5>
                                <i className="fas fa-download"></i> <a href="">Transmissor</a>
                            </h5>
                        </Box>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <Box>
                            <div align="center">
                                <img src="./imagens/EsquemaEFD-Reinf.png" alt="Esquema Intech EFD-Reinf" width="593" height="1048" />
                            </div>
                        </Box>
                    </Col>
                </Row>
            </Page>
        );
    }

}