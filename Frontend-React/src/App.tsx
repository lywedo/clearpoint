import './App.css'
import { Container, Row } from 'react-bootstrap'
import { ProjectDescription } from './components/ProjectDescription'
import { TodoItems, AddTodoItem } from './components/TodoItem'
import { Footer } from './components/Footer'


//const axios = require('axios')

const App = () => {
  return (
    <div className="App">
      <Container>
        <ProjectDescription />
        <Row>
          <AddTodoItem />
        </Row>
        <br />
        <Row>
          <TodoItems />
        </Row>
      </Container>
      <Footer />
    </div>
  )
}

export default App
